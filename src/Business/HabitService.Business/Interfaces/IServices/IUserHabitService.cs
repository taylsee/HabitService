using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.IServices
{
    public interface IUserHabitService
    {
        Task<List<UserHabit>> GetUserHabitsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserHabit> AddHabitToUserAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default);
        Task<UserHabit> UpdateProgressAsync(Guid userHabitId, int newValue, CancellationToken cancellationToken = default);
        Task CompleteHabitAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task RemoveHabitFromUserAsync(Guid userHabitId, CancellationToken cancellationToken = default);
        Task<UserHabit?> GetUserHabitByIdAsync(Guid userHabitId, CancellationToken cancellationToken = default);
    }
}
