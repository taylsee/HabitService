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
        Task<List<Habit>> GetPredefinedHabitsAsync();
        Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId);
        Task<Habit> CreateCustomHabitAsync(Guid userId, string name, string description,
            int PeriodInDays, int targetValue);
        Task DeleteCustomHabitAsync(Guid userId, Guid habitId);
        Task<Habit?> GetHabitByIdAsync(Guid habitId);
    }
}
