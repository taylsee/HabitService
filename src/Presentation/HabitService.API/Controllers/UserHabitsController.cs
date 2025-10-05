using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    /// <summary>
    /// Контроллер для управления привычками пользователей
    /// </summary>
    [ApiController]
    [Route("api/users/{userId}/habits")]
    public class UserHabitsController : ControllerBase
    {
        private readonly IUserHabitService _userHabitService;
        private readonly IHabitCompletionService _completionService;
        private readonly IMapper _mapper;

        public UserHabitsController(IUserHabitService userHabitService,
            IHabitCompletionService completionService,
            IMapper mapper)
        {
            _userHabitService = userHabitService;
            _completionService = completionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все привычки пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список привычек пользователя с прогрессом</returns>
        /// <response code="200">Успешное получение списка привычек</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserHabitResponse>), 200)]
        public async Task<ActionResult<List<UserHabitResponse>>> GetUserHabits(Guid userId, CancellationToken cancellationToken = default)
        {
            var userHabits = await _userHabitService.GetUserHabitsAsync(userId, cancellationToken);

            var response = new List<UserHabitResponse>();
            foreach (var userHabit in userHabits)
            {
                var item = await MapToUserHabitResponse(userHabit, cancellationToken);
                response.Add(item);
            }

            return Ok(response);
        }

        /// <summary>
        /// Получить конкретную привычку пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Данные привычки пользователя с прогрессом</returns>
        /// <response code="200">Успешное получение привычки</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpGet("{userHabitId}")]
        [ProducesResponseType(typeof(UserHabitResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserHabitResponse>> GetUserHabitById(Guid userId, Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
            if (userHabit == null || userHabit.UserId != userId)
                return NotFound();

            var response = await MapToUserHabitResponse(userHabit, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Добавить привычку пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="habitId">Идентификатор привычки (системной или кастомной)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданная пользовательская привычка</returns>
        /// <response code="201">Привычка успешно добавлена пользователю</response>
        /// <response code="400">Неверные данные запроса</response>
        [HttpPost("add/{habitId}")]
        [ProducesResponseType(typeof(UserHabitResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserHabitResponse>> AddHabitToUser(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userHabit = await _userHabitService.AddHabitToUserAsync(userId, habitId, cancellationToken);
                var response = await MapToUserHabitResponse(userHabit, cancellationToken);
                return CreatedAtAction(
                    nameof(GetUserHabitById),
                    new { userId, userHabitId = userHabit.Id },
                    response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить привычку у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        /// <response code="204">Привычка успешно удалена</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpDelete("{userHabitId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveHabitFromUser(Guid userId, Guid userHabitId, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
                if (userHabit == null || userHabit.UserId != userId)
                    return NotFound();

                await _userHabitService.RemoveHabitFromUserAsync(userHabitId, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Маппинг пользовательской привычки в DTO с прогрессом
        /// </summary>
        /// <param name="userHabit">Пользовательская привычка</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO пользовательской привычки с данными о прогрессе</returns>
        private async Task<UserHabitResponse> MapToUserHabitResponse(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            var progress = await _completionService.GetCurrentProgressAsync(userHabit.Id, cancellationToken);
            var response = _mapper.Map<UserHabitResponse>(userHabit);

            response.CurrentProgress = progress.CurrentValue;
            response.IsCompleted = progress.IsCompleted;
            response.Remaining = progress.Remaining;
            response.ProgressPercentage = progress.ProgressPercentage;

            return response;
        }
    }
}