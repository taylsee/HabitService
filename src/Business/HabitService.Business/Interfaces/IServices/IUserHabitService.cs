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
        Task<List<UserHabit>> GetUserHabitsAsync(Guid userId);
        Task<UserHabit> AddHabitToUserAsync(Guid userId, Guid habitId);
        Task<UserHabit> UpdateProgressAsync(Guid userHabitId, int newValue);
        Task CompleteHabitAsync(Guid userHabitId);
        Task RemoveHabitFromUserAsync(Guid userHabitId);
        Task<UserHabit?> GetUserHabitByIdAsync(Guid userHabitId);
    }
}
