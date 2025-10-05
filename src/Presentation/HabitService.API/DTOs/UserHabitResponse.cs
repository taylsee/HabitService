namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO с данными пользовательской привычки
    /// </summary>
    public class UserHabitResponse
    {
        /// <summary>
        /// Идентификатор записи привычки у пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор привычки
        /// </summary>
        public Guid HabitId { get; set; }

        /// <summary>
        /// Данные базовой привычки
        /// </summary>
        public HabitResponse Habit { get; set; } = null!;

        /// <summary>
        /// Дата начала отслеживания
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Флаг активности привычки
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Текущий прогресс выполнения
        /// </summary>
        public int CurrentProgress { get; set; }

        /// <summary>
        /// Флаг завершения привычки
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Оставшееся значение для завершения
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// Процент выполнения (0-100)
        /// </summary>
        public double ProgressPercentage { get; set; }
    }
}