using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    /// <summary>
    /// Контроллер для управления привычками
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitCatalogService _habitCatalogService;
        private readonly IMapper _mapper;

        public HabitsController(IHabitCatalogService habitCatalogService, IMapper mapper)
        {
            _habitCatalogService = habitCatalogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список предопределенных (системных) привычек
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список системных привычек</returns>
        /// <response code="200">Успешное получение списка привычек</response>
        [HttpGet("predefined")]
        [ProducesResponseType(typeof(List<HabitResponse>), 200)]
        public async Task<ActionResult<List<HabitResponse>>> GetPredefinedHabits(CancellationToken cancellationToken = default)
        {
            var habits = await _habitCatalogService.GetPredefinedHabitsAsync(cancellationToken);
            var response = _mapper.Map<List<HabitResponse>>(habits);
            return Ok(response);
        }

        /// <summary>
        /// Получить привычки, созданные пользователем
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список кастомных привычек пользователя</returns>
        /// <response code="200">Успешное получение списка привычек</response>
        /// <response code="400">Неверный запрос</response>
        [HttpGet("user/{userId}/custom")]
        [ProducesResponseType(typeof(List<HabitResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<HabitResponse>>> GetUserCustomHabits(Guid userId, CancellationToken cancellationToken = default)
        {
            var habits = await _habitCatalogService.GetUserCustomHabitsAsync(userId, cancellationToken);
            var response = _mapper.Map<List<HabitResponse>>(habits);
            return Ok(response);
        }

        /// <summary>
        /// Получить привычку по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="habitId">Идентификатор привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Данные привычки</returns>
        /// <response code="200">Успешное получение привычки</response>
        /// <response code="404">Привычка не найдена</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpGet("user/{userId}/{habitId}")]
        [ProducesResponseType(typeof(HabitResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<HabitResponse>> GetHabitById(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            var habit = await _habitCatalogService.GetHabitByIdAsync(habitId, cancellationToken);
            if (habit == null)
                return NotFound();

            if (habit.CreatedBy.HasValue && habit.CreatedBy != userId)
                return Forbid();

            var response = _mapper.Map<HabitResponse>(habit);
            return Ok(response);
        }

        /// <summary>
        /// Создать привычку для пользователя (привычка лишь создается в таблице Habit, она не привязывается к пользователю в таблице UserHabit)
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="request">Данные для создания привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданная привычка</returns>
        /// <response code="201">Привычка успешно создана</response>
        /// <response code="400">Неверные данные запроса</response>
        [HttpPost("user/{userId}/custom")]
        [ProducesResponseType(typeof(HabitResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HabitResponse>> CreateCustomHabit(
            Guid userId,
            [FromBody] CreateUpdateHabitRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var habit = _mapper.Map<Habit>(request);
                var createdHabit = await _habitCatalogService.CreateCustomHabitAsync(
                    userId,
                    habit.Name,
                    habit.Description,
                    habit.PeriodInDays,
                    habit.TargetValue,
                    cancellationToken
                );

                var response = _mapper.Map<HabitResponse>(createdHabit);
                return CreatedAtAction(nameof(GetHabitById), new { userId = userId, habitId = createdHabit.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить значения у существующуей привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="habitId">Идентификатор привычки</param>
        /// <param name="request">Данные для обновления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновленная привычка</returns>
        /// <response code="200">Привычка успешно обновлена</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Привычка не найдена</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpPut("user/{userId}/{habitId}")]
        [ProducesResponseType(typeof(HabitResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<HabitResponse>> UpdateHabit(
            Guid userId,
            Guid habitId,
            [FromBody] CreateUpdateHabitRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingHabit = await _habitCatalogService.GetHabitByIdAsync(habitId, cancellationToken);
                if (existingHabit == null)
                    return NotFound();

                if (existingHabit.CreatedBy.HasValue && existingHabit.CreatedBy != userId)
                    return Forbid();

                if (!existingHabit.CreatedBy.HasValue)
                    return BadRequest(new { error = "Cannot update predefined habit" });

                await _habitCatalogService.UpdateHabitAsync(
                    habitId,
                    request.Name,
                    request.Description,
                    request.PeriodInDays,
                    request.TargetValue,
                    cancellationToken);

                var updatedHabit = await _habitCatalogService.GetHabitByIdAsync(habitId, cancellationToken);
                var response = _mapper.Map<HabitResponse>(updatedHabit);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить привычку, созданную пользователем
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="habitId">Идентификатор привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        /// <response code="204">Привычка успешно удалена</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Привычка не найдена</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpDelete("user/{userId}/custom/{habitId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteCustomHabit(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingHabit = await _habitCatalogService.GetHabitByIdAsync(habitId, cancellationToken);
                if (existingHabit == null)
                    return NotFound();

                if (!existingHabit.CreatedBy.HasValue || existingHabit.CreatedBy != userId)
                    return Forbid();

                await _habitCatalogService.DeleteCustomHabitAsync(userId, habitId, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}