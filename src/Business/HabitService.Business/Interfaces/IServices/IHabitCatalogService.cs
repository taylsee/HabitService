using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.IServices
{
    public interface IHabitCatalogService
    {
        Task<List<Habit>> GetPredefinedHabitsAsync(CancellationToken cancellationToken = default);
        Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Habit> CreateCustomHabitAsync(Guid userId, string name, string description,
            int PeriodInDays, int targetValue, CancellationToken cancellationToken = default);
        Task DeleteCustomHabitAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default);
        Task<Habit?> GetHabitByIdAsync(Guid habitId, CancellationToken cancellationToken = default);
    }
}
