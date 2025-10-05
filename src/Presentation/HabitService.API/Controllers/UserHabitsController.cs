using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserHabitsController(IUserHabitService userHabitService, 
            IHabitCompletionService completionService,
            IMapper mapper)
        {
            _userHabitService = userHabitService;
            _completionService = completionService;
            _mapper = mapper;
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

        [HttpGet("{userHabitId}")]
        public async Task<ActionResult<UserHabitResponse>> GetUserHabitById(Guid userId, Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
            if (userHabit == null || userHabit.UserId != userId)
                return NotFound();

            var response = await MapToUserHabitResponse(userHabit, cancellationToken);
            return Ok(response);
        }

        [HttpPost("add/{habitId}")]
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

        [HttpDelete("{userHabitId}")]
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

