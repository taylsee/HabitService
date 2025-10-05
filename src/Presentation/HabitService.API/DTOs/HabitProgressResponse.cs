namespace HabitService.API.DTOs
{
    /// <summary>
    /// DTO с прогрессом выполнения привычки
    /// </summary>
    public class HabitProgressResponse
    {
        /// <summary>
        /// Текущее значение прогресса
        /// </summary>
        public int CurrentValue { get; set; }

        /// <summary>
        /// Целевое значение
        /// </summary>
        public int TargetValue { get; set; }

        /// <summary>
        /// Оставшееся значение для завершения
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// Процент выполнения (0-100)
        /// </summary>
        public double ProgressPercentage { get; set; }

        /// <summary>
        /// Флаг завершения привычки
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Начало периода отслеживания
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// Конец периода отслеживания
        /// </summary>
        public DateTime PeriodEnd { get; set; }
    }
}