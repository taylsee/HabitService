using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.IServices
{
    public interface IHabitCompletionService
    {
        Task<HabitCompletion> CompleteHabitAsync(Guid userHabitId, int value = 1, string? notes = null, CancellationToken cancellationToken = default);
        Task<List<HabitCompletion>> GetCompletionsAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task<HabitProgress> GetCurrentProgressAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task<bool> CanCompleteHabitTodayAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task ResetHabitProgressAsync(UserHabit userHabit, CancellationToken cancellationToken = default);
        Task<bool> ResetIfPeriodEndedAsync(UserHabit userHabit, CancellationToken cancellationToken = default);
        Task DeleteCompletionAsync(Guid completionId, CancellationToken cancellationToken = default);
        Task<HabitCompletion?> GetCompletionByIdAsync(Guid completionId, CancellationToken cancellationToken = default);
        Task UpdateCompletionAsync(Guid completionId, int value, string? notes, CancellationToken cancellationToken = default);
    }

    public class HabitProgress
    {
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }
        public int Remaining => Math.Max(0, TargetValue - CurrentValue);
        public double ProgressPercentage => TargetValue > 0 ? (double)CurrentValue / TargetValue * 100 : 0;
        public bool IsCompleted => CurrentValue >= TargetValue;
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
