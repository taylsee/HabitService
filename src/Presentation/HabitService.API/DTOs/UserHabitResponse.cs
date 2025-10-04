namespace HabitService.API.DTOs
{
    public class UserHabitResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid HabitId { get; set; }
        public HabitResponse Habit { get; set; } = null!;
        public int CurrentValue { get; set; }
        public int ProgressPercentage => Habit.TargetValue > 0 ?
            (int)((double)CurrentValue / Habit.TargetValue * 100) : 0;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
    }
}
