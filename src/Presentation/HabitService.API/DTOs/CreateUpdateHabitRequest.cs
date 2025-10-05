using System.ComponentModel.DataAnnotations;

namespace HabitService.API.DTOs
{
    public class CreateUpdateHabitRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
        [Range(1, 365, ErrorMessage = "Period must be between 1 and 365 days")]
        public int PeriodInDays { get; set; } = 7;
        [Range(1, 1000000000, ErrorMessage = "Target value must be between 1 and 1000000000")]
        public int TargetValue { get; set; }
    }
}
