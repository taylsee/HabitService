using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HabitService.API.Controllers
{
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

        private async Task<bool> IsUserHabitOwnerAsync(Guid userId, Guid userHabitId, CancellationToken cancellationToken)
        {
            var userHabit = await _userHabitService.GetUserHabitByIdAsync(userHabitId, cancellationToken);
            return userHabit != null && userHabit.UserId == userId;
        }

        [HttpGet]
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

        [HttpGet("{completionId}")]
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

        [HttpGet("progress")]
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

        [HttpPost]
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

        [HttpPut("reset")]
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

        [HttpPut("{completionId}")]
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

        [HttpDelete("{completionId}")]
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
