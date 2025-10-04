using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.IRepositories
{
    public interface IHabitCompletionRepository
    {
        Task<HabitCompletion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<HabitCompletion>> GetByUserHabitIdAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task<List<HabitCompletion>> GetByUserHabitIdAndDateRangeAsync(Guid userHabitId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<HabitCompletion> AddAsync(HabitCompletion completion, CancellationToken cancellationToken = default);
        Task UpdateAsync(HabitCompletion completion, CancellationToken cancellationToken = default);
        Task DeleteAsync(HabitCompletion completion, CancellationToken cancellationToken = default);
        Task<int> GetCompletionsCountForPeriodAsync(Guid userHabitId, DateTime periodStart, CancellationToken cancellationToken = default);
        Task<bool> HasCompletionTodayAsync(Guid userHabitId, CancellationToken cancellationToken = default);
    }
}
