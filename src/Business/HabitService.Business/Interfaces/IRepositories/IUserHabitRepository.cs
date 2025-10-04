using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Interfaces.Repositories
{

    public interface IUserHabitRepository
    {
        Task<UserHabit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<UserHabit>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserHabit?> GetByUserAndHabitIdAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default);
        Task<UserHabit> AddAsync(UserHabit userHabit, CancellationToken cancellationToken = default);
        Task UpdateAsync(UserHabit userHabit, CancellationToken cancellationToken = default);
        Task DeleteAsync(UserHabit userHabit, CancellationToken cancellationToken = default);
        Task<bool> UserHasHabitAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default);
    }
}
