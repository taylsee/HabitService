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

        [HttpGet("user/{userId}/{habitId}")]
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


        [HttpPost("user/{userId}/custom")]
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
                return CreatedAtAction(nameof(GetHabitById), new { habitId = createdHabit.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("user/{userId}/{habitId}")]
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

        [HttpDelete("user/{userId}/custom/{habitId}")]
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
