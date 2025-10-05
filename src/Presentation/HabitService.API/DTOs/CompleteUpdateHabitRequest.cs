using System.ComponentModel.DataAnnotations;

namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO для выполнения или обновления выполнения привычки
    /// </summary>
    public class CompleteUpdateHabitRequest
    {
        /// <summary>
        /// Значение выполнения
        /// </summary>
        /// <example>1</example>
        [Range(1, 1000000000, ErrorMessage = "Value must be between 1 and 1000000000")]
        public int Value { get; set; } = 1;

        /// <summary>
        /// Заметки к выполнению
        /// </summary>
        /// <example>Сегодня было легко выполнить</example>
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}