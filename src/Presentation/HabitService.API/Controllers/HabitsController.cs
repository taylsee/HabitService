using HabitService.API.DTOs;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Models;
using HabitService.Business.Services;
using Microsoft.AspNetCore.Mvc;


namespace HabitService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitCatalogService _habitCatalogService;

        public HabitsController(IHabitCatalogService habitCatalogService)
        {
            _habitCatalogService = habitCatalogService;
        }

        [HttpGet("predefined")]
        public async Task<ActionResult<List<HabitResponse>>> GetPredefinedHabits()
        {
            var habits = await _habitCatalogService.GetPredefinedHabitsAsync();
            var response = habits.Select(MapToHabitResponse).ToList();
            return Ok(response);
        }

        [HttpGet("user/{userId}/custom")]
        public async Task<ActionResult<List<HabitResponse>>> GetUserCustomHabits(Guid userId)
        {
            var habits = await _habitCatalogService.GetUserCustomHabitsAsync(userId);
            var response = habits.Select(MapToHabitResponse).ToList();
            return Ok(response);
        }

        [HttpPost("user/{userId}/custom")]
        public async Task<ActionResult<HabitResponse>> CreateCustomHabit(
            Guid userId,
            [FromBody] CreateHabitRequest request)
        {
            try
            {
                var habit = await _habitCatalogService.CreateCustomHabitAsync(
                    userId,
                    request.Name,
                    request.Description,
                    request.PeriodInDays,
                    request.TargetValue
                    );

                var response = MapToHabitResponse(habit);
                return CreatedAtAction(nameof(GetHabitById), new { id = habit.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HabitResponse>> GetHabitById(Guid id)
        {
            var habit = await _habitCatalogService.GetHabitByIdAsync(id);
            if (habit == null)
                return NotFound();

            var response = MapToHabitResponse(habit);
            return Ok(response);
        }

        [HttpDelete("user/{userId}/custom/{habitId}")]
        public async Task<IActionResult> DeleteCustomHabit(Guid userId, Guid habitId)
        {
            try
            {
                await _habitCatalogService.DeleteCustomHabitAsync(userId, habitId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private static HabitResponse MapToHabitResponse(Habit habit)
        {
            return new HabitResponse
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                PeriodInDays = habit.PeriodInDays,
                TargetValue = habit.TargetValue,
                CreatedAt = habit.CreatedAt
            };
        }
    }
}
