using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO для создания или обновления привычки
    /// </summary>
    public class CreateUpdateHabitRequest
    {
        /// <summary>
        /// Название привычки
        /// </summary>
        /// <example>Бег</example>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание привычки
        /// </summary>
        /// <example>Бегать 3 раза в неделю</example>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Период выполнения в днях
        /// </summary>
        /// <example>Бегать 3 раза в неделю, PeriodInDays = 7</example>
        [Range(1, 365, ErrorMessage = "Period must be between 1 and 365 days")]
        [DefaultValue(7)]
        public int PeriodInDays { get; set; } = 7;

        /// <summary>
        /// Целевое значение для выполнения
        /// </summary>
        /// <example>Бегать 3 раза в неделю, TargetValue = 3</example>
        [Range(1, 1000000000, ErrorMessage = "Target value must be between 1 and 1000000000")]
        [DefaultValue(3)]
        public int TargetValue { get; set; }
    }
}