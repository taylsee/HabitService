using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    /// <summary>
    /// Контроллер для управления выполнениями привычек
    /// </summary>
    [ApiController]
    [Route("api/users/{userId}/habits/{userHabitId}/completions")]
    public class HabitCompletionsController : ControllerBase
    {
        private readonly IHabitCompletionService _completionService;
        private readonly IUserHabitService _userHabitService;
        private readonly IMapper _mapper;

        public HabitCompletionsController(IHabitCompletionService completionService,
            IUserHabitService userHabitService,
            IMapper mapper)
        {
            _completionService = completionService;
            _userHabitService = userHabitService;
            _mapper = mapper;
        }

        /// <summary>
        /// Проверяет принадлежность пользовательской привычки пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если привычка принадлежит пользователю</returns>
        private async Task<bool> IsUserHabitOwnerAsync(Guid userId, Guid userHabitId, CancellationToken cancellationToken)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
            return userHabit != null && userHabit.UserId == userId;
        }

        /// <summary>
        /// Получить список всех выполнений привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Список выполнений привычки</returns>
        /// <response code="200">Успешное получение списка выполнений</response>
        /// <response code="400">Ошибка в запросе</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<HabitCompletionResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<HabitCompletionResponse>>> GetCompletions(
            Guid userId,
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var completions = await _completionService.GetCompletionsAsync(userHabitId, cancellationToken);
                var response = _mapper.Map<List<HabitCompletionResponse>>(completions);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получить конкретное выполнение привычки по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="completionId">Идентификатор выполнения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Данные выполнения привычки</returns>
        /// <response code="200">Успешное получение выполнения</response>
        /// <response code="400">Ошибка в запросе</response>
        /// <response code="404">Выполнение не найдено или не принадлежит привычке</response>
        [HttpGet("{completionId}")]
        [ProducesResponseType(typeof(HabitCompletionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitCompletionResponse>> GetCompletionById(
            Guid userId,
            Guid userHabitId,
            Guid completionId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var completion = await _completionService.GetCompletionByIdAsync(completionId, cancellationToken);
                if (completion == null || completion.UserHabitId != userHabitId)
                    return NotFound();

                var response = _mapper.Map<HabitCompletionResponse>(completion);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получить текущий прогресс выполнения привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Текущий прогресс выполнения привычки</returns>
        /// <response code="200">Успешное получение прогресса</response>
        /// <response code="400">Ошибка в запросе</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpGet("progress")]
        [ProducesResponseType(typeof(HabitProgressResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitProgressResponse>> GetCurrentProgress(
            Guid userId,
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var progress = await _completionService.GetCurrentProgressAsync(userHabitId, cancellationToken);
                var response = _mapper.Map<HabitProgressResponse>(progress);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Зафиксировать выполнение привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="request">Данные выполнения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданное выполнение привычки</returns>
        /// <response code="200">Успешное выполнение привычки</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpPost]
        [ProducesResponseType(typeof(HabitCompletionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitCompletionResponse>> CompleteHabit(
            Guid userId,
            Guid userHabitId,
            [FromBody] CompleteUpdateHabitRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var completion = await _completionService.CompleteHabitAsync(
                    userHabitId,
                    request.Value,
                    request.Notes,
                    cancellationToken);

                var response = _mapper.Map<HabitCompletionResponse>(completion);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Сбросить прогресс выполнения привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        /// <response code="204">Прогресс успешно сброшен</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Привычка не найдена или не принадлежит пользователю</response>
        [HttpPut("reset")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ResetProgress(
            Guid userId,
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
                if (userHabit == null || userHabit.UserId != userId)
                    return NotFound();

                await _completionService.ResetHabitProgressAsync(userHabit, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные выполнения привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="completionId">Идентификатор выполнения</param>
        /// <param name="request">Новые данные выполнения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновленное выполнение привычки</returns>
        /// <response code="200">Успешное обновление выполнения</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Выполнение не найдено или не принадлежит привычке</response>
        [HttpPut("{completionId}")]
        [ProducesResponseType(typeof(HabitCompletionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitCompletionResponse>> UpdateCompletion(
            Guid userId,
            Guid userHabitId,
            Guid completionId,
            [FromBody] CompleteUpdateHabitRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var completion = await _completionService.GetCompletionByIdAsync(completionId, cancellationToken);
                if (completion == null || completion.UserHabitId != userHabitId)
                    return NotFound();

                await _completionService.UpdateCompletionAsync(
                    completionId,
                    request.Value,
                    request.Notes,
                    cancellationToken);

                var updatedCompletion = await _completionService.GetCompletionByIdAsync(completionId, cancellationToken);
                var response = _mapper.Map<HabitCompletionResponse>(updatedCompletion);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить выполнение привычки
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="userHabitId">Идентификатор пользовательской привычки</param>
        /// <param name="completionId">Идентификатор выполнения</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Результат операции</returns>
        /// <response code="204">Выполнение успешно удалено</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="404">Выполнение не найдено или не принадлежит привычке</response>
        [HttpDelete("{completionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCompletion(
            Guid userId,
            Guid userHabitId,
            Guid completionId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!await IsUserHabitOwnerAsync(userId, userHabitId, cancellationToken))
                    return NotFound();

                var completion = await _completionService.GetCompletionByIdAsync(completionId, cancellationToken);
                if (completion == null || completion.UserHabitId != userHabitId)
                    return NotFound();

                await _completionService.DeleteCompletionAsync(completionId, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}