using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/habits")]
    public class UserHabitsController : ControllerBase
    {
        private readonly IUserHabitService _userHabitService;
        private readonly IHabitCompletionService _completionService;

        public UserHabitsController(IUserHabitService userHabitService, IHabitCompletionService completionService)
        {
            _userHabitService = userHabitService;
            _completionService = completionService;
        }

        [HttpGet]
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

        [HttpPost("add/{habitId}")]
        public async Task<ActionResult<UserHabitResponse>> AddHabitToUser(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
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

        [HttpDelete("{userHabitId}")]
        public async Task<IActionResult> RemoveHabitFromUser(Guid userId, Guid userHabitId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userHabitService.RemoveHabitFromUserAsync(userHabitId, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{userHabitId}")]
        public async Task<ActionResult<UserHabitResponse>> GetUserHabitById(Guid userId, Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
            if (userHabit == null || userHabit.UserId != userId)
                return NotFound();

            var response = await MapToUserHabitResponse(userHabit, cancellationToken);
            return Ok(response);
        }

        private async Task<UserHabitResponse> MapToUserHabitResponse(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            var progress = await _completionService.GetCurrentProgressAsync(userHabit.Id, cancellationToken);

            return new UserHabitResponse
            {
                Id = userHabit.Id,
                UserId = userHabit.UserId,
                HabitId = userHabit.HabitId,
                Habit = new HabitResponse
                {
                    Id = userHabit.Habit.Id,
                    Name = userHabit.Habit.Name,
                    Description = userHabit.Habit.Description,
                    PeriodInDays = userHabit.Habit.PeriodInDays,
                    TargetValue = userHabit.Habit.TargetValue,
                    CreatedAt = userHabit.Habit.CreatedAt
                },
                StartDate = userHabit.StartDate,
                IsActive = userHabit.IsActive,
                CurrentProgress = progress.CurrentValue,
                IsCompleted = progress.IsCompleted,
                Remaining = progress.Remaining,
                ProgressPercentage = progress.ProgressPercentage
            };
        }
    }
}

