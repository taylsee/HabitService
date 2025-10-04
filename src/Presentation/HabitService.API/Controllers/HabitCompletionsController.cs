using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    [ApiController]
    [Route("api/user-habits/{userHabitId}/completions")]
    public class HabitCompletionsController : ControllerBase
    {
        private readonly IHabitCompletionService _completionService;
        private readonly IUserHabitService _userHabitService;

        public HabitCompletionsController(IHabitCompletionService completionService, IUserHabitService userHabitService)
        {
            _completionService = completionService;
            _userHabitService = userHabitService;
        }

        [HttpPost]
        public async Task<ActionResult<HabitCompletionResponse>> CompleteHabit(
            Guid userHabitId,
            [FromBody] CompleteHabitRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var completion = await _completionService.CompleteHabitAsync(
                    userHabitId,
                    request.Value,
                    request.Notes,
                    cancellationToken);

                var response = new HabitCompletionResponse
                {
                    Id = completion.Id,
                    UserHabitId = completion.UserHabitId,
                    CompletedAt = completion.CompletedAt,
                    Value = completion.Value,
                    Notes = completion.Notes
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<HabitCompletionResponse>>> GetCompletions(
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var completions = await _completionService.GetCompletionsAsync(userHabitId, cancellationToken);
                var response = completions.Select(c => new HabitCompletionResponse
                {
                    Id = c.Id,
                    UserHabitId = c.UserHabitId,
                    CompletedAt = c.CompletedAt,
                    Value = c.Value,
                    Notes = c.Notes
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("progress")]
        public async Task<ActionResult<HabitProgressResponse>> GetCurrentProgress(
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var progress = await _completionService.GetCurrentProgressAsync(userHabitId, cancellationToken);
                var response = new HabitProgressResponse
                {
                    CurrentValue = progress.CurrentValue,
                    TargetValue = progress.TargetValue,
                    Remaining = progress.Remaining,
                    ProgressPercentage = progress.ProgressPercentage,
                    IsCompleted = progress.IsCompleted,
                    PeriodStart = progress.PeriodStart,
                    PeriodEnd = progress.PeriodEnd
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("can-complete-today")]
        public async Task<ActionResult<bool>> CanCompleteHabitToday(
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canComplete = await _completionService.CanCompleteHabitTodayAsync(userHabitId, cancellationToken);
                return Ok(canComplete);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetProgress(
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
                if (userHabit == null)
                    return NotFound();
                await _completionService.ResetHabitProgressAsync(userHabit, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
