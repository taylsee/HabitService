using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/habits")]
    public class UserHabitsController : ControllerBase
    {
        private readonly IUserHabitService _userHabitService;

        public UserHabitsController(IUserHabitService userHabitService)
        {
            _userHabitService = userHabitService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserHabitResponse>>> GetUserHabits(Guid userId)
        {
            var userHabits = await _userHabitService.GetUserHabitsAsync(userId);
            var response = userHabits.Select(MapToUserHabitResponse).ToList();
            return Ok(response);
        }

        [HttpPost("add/{habitId}")]
        public async Task<ActionResult<UserHabitResponse>> AddHabitToUser(Guid userId, Guid habitId)
        {
            try
            {
                var userHabit = await _userHabitService.AddHabitToUserAsync(userId, habitId);
                var response = MapToUserHabitResponse(userHabit);
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

        [HttpPut("{userHabitId}/progress")]
        public async Task<ActionResult<UserHabitResponse>> UpdateProgress(
            Guid userId,
            Guid userHabitId,
            [FromBody] UpdateProgressRequest request)
        {
            try
            {
                var userHabit = await _userHabitService.UpdateProgressAsync(userHabitId, request.NewValue);
                var response = MapToUserHabitResponse(userHabit);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{userHabitId}/complete")]
        public async Task<IActionResult> CompleteHabit(Guid userId, Guid userHabitId)
        {
            try
            {
                await _userHabitService.CompleteHabitAsync(userHabitId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{userHabitId}")]
        public async Task<IActionResult> RemoveHabitFromUser(Guid userId, Guid userHabitId)
        {
            try
            {
                await _userHabitService.RemoveHabitFromUserAsync(userHabitId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{userHabitId}")]
        public async Task<ActionResult<UserHabitResponse>> GetUserHabitById(Guid userId, Guid userHabitId)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId);
            if (userHabit == null || userHabit.UserId != userId)
                return NotFound();

            var response = MapToUserHabitResponse(userHabit);
            return Ok(response);
        }

        private static UserHabitResponse MapToUserHabitResponse(UserHabit userHabit)
        {
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
                CurrentValue = userHabit.CurrentValue,
                StartDate = userHabit.StartDate,
                IsActive = userHabit.IsActive,
                Completions = userHabit.Completions.Select(c => new HabitCompletionResponse
                {
                    CompletedDate = c.CompletedDate,
                    CompletedValue = c.CompletedValue
                }).ToList()
            };
        }
    }
}

