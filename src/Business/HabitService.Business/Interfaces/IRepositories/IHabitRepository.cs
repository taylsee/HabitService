using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.Repositories 
{
    public interface IHabitRepository
    {
        Task<List<Habit>> GetPredefinedHabitsAsync(CancellationToken cancellationToken = default);
        Task<Habit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Habit> AddAsync(Habit habit, CancellationToken cancellationToken = default);
        Task UpdateAsync(Habit habit, CancellationToken cancellationToken = default);
        Task DeleteAsync(Habit habit, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
