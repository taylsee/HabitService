namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO с данными привычки
    /// </summary>
    public class HabitResponse
    {
        /// <summary>
        /// Идентификатор привычки
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название привычки
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание привычки
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Период выполнения в днях
        /// </summary>
        /// <example>Бегать 3 раза в неделю, PeriodInDays = 7</example>
        public int PeriodInDays { get; set; } = 7;

        /// <summary>
        /// Целевое значение для выполнения
        /// </summary>
        /// <example>Бегать 3 раза в неделю, TargetValue = 3</example>
        public int TargetValue { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}