namespace HabitService.API.DTOs
{
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
