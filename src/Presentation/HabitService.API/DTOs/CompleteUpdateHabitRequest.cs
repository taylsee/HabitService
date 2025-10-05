using System.ComponentModel.DataAnnotations;

namespace HabitService.API.DTOs
{
    public class CompleteUpdateHabitRequest
    {
        [Range(1, 1000000000, ErrorMessage = "Value must be between 1 and 1000000000")]
        public int Value { get; set; } = 1;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
