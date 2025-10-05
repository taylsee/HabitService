using AutoMapper;
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
        private readonly IMapper _mapper;

        public HabitCompletionsController(IHabitCompletionService completionService, 
            IUserHabitService userHabitService,
            IMapper mapper)
        {
            _completionService = completionService;
            _userHabitService = userHabitService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<HabitCompletionResponse>>> GetCompletions(
            Guid userHabitId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var completions = await _completionService.GetCompletionsAsync(userHabitId, cancellationToken);
                var response = _mapper.Map<List<HabitCompletionResponse>>(completions);
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

        [HttpDelete("{completionId}")]
        public async Task<IActionResult> DeleteCompletion(
        Guid userHabitId,
        Guid completionId,
        CancellationToken cancellationToken = default)
        {
            try
            {
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
