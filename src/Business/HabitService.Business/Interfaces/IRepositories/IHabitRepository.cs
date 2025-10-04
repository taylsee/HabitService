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
        Task<List<Habit>> GetPredefinedHabitsAsync();
        Task<Habit?> GetByIdAsync(Guid id);
        Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId);
        Task<Habit> AddAsync(Habit habit);
        Task UpdateAsync(Habit habit);
        Task DeleteAsync(Habit habit);
        Task<bool> ExistsAsync(Guid id);
    }
}
