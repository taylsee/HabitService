using AutoMapper;
using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using Microsoft.AspNetCore.Mvc;


namespace HabitService.API.Controllers
{
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

        [HttpGet("predefined")]
        public async Task<ActionResult<List<HabitResponse>>> GetPredefinedHabits(CancellationToken cancellationToken = default)
        {
            var habits = await _habitCatalogService.GetPredefinedHabitsAsync(cancellationToken);
            var response = _mapper.Map<List<HabitResponse>>(habits);
            return Ok(response);
        }

        [HttpGet("user/{userId}/custom")]
        public async Task<ActionResult<List<HabitResponse>>> GetUserCustomHabits(Guid userId, CancellationToken cancellationToken = default)
        {
            var habits = await _habitCatalogService.GetUserCustomHabitsAsync(userId, cancellationToken);
            var response = _mapper.Map<List<HabitResponse>>(habits);
            return Ok(response);
        }

        [HttpGet("{habitId}")]
        public async Task<ActionResult<HabitResponse>> GetHabitById(Guid habitId, CancellationToken cancellationToken = default)
        {
            var habit = await _habitCatalogService.GetHabitByIdAsync(habitId, cancellationToken);
            if (habit == null)
                return NotFound();

            var response = _mapper.Map<HabitResponse>(habit);
            return Ok(response);
        }

        [HttpPost("user/{userId}/custom")]
        public async Task<ActionResult<HabitResponse>> CreateCustomHabit(
            Guid userId,
            [FromBody] CreateHabitRequest request,
            CancellationToken cancellationToken = default)
        {
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
                return CreatedAtAction(nameof(GetHabitById), new { habitId = createdHabit.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("user/{userId}/custom/{habitId}")]
        public async Task<IActionResult> DeleteCustomHabit(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            try
            {
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
