namespace HabitService.API.DTOs
{
    public class HabitCompletionResponse
    {
        public Guid Id { get; set; }
        public Guid UserHabitId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int Value { get; set; }
        public string? Notes { get; set; }
    }
}
