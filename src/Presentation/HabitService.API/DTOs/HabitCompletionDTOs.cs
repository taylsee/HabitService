namespace HabitService.API.DTOs
{
    public class CompleteHabitRequest
    {
        public int Value { get; set; } = 1;
        public string? Notes { get; set; }
    }

    public class HabitCompletionResponse
    {
        public Guid Id { get; set; }
        public Guid UserHabitId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int Value { get; set; }
        public string? Notes { get; set; }
    }

    public class HabitProgressResponse
    {
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }
        public int Remaining { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
