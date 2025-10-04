namespace HabitService.API.DTOs
{
    public class UserHabitResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid HabitId { get; set; }
        public HabitResponse Habit { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsCompleted { get; set; }
        public int Remaining { get; set; }
        public double ProgressPercentage { get; set; }
    }
}
