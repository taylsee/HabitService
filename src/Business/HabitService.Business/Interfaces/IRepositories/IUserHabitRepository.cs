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
        Task<UserHabit?> GetByIdAsync(Guid id);
        Task<List<UserHabit>> GetByUserIdAsync(Guid userId);
        Task<UserHabit?> GetByUserAndHabitIdAsync(Guid userId, Guid habitId);
        Task<UserHabit> AddAsync(UserHabit userHabit);
        Task UpdateAsync(UserHabit userHabit);
        Task DeleteAsync(UserHabit userHabit);
        Task<bool> UserHasHabitAsync(Guid userId, Guid habitId);
    }
}
