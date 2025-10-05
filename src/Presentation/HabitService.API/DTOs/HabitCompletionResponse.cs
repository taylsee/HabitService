namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO с данными о выполнении привычки
    /// </summary>
    public class HabitCompletionResponse
    {
        /// <summary>
        /// Идентификатор выполнения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор записи привычки у пользователя
        /// </summary>
        public Guid UserHabitId { get; set; }

        /// <summary>
        /// Дата и время выполнения
        /// </summary>
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// Значение выполнения
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Заметки
        /// </summary>
        public string? Notes { get; set; }
    }
}