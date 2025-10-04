namespace HabitService.API.DTOs
{
    public class HabitResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PeriodInDays { get; set; } = 7;
        public int TargetValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
