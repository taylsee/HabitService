using HabitService.Business.Interfaces.Repositories;
using HabitService.Business.Models;
using HabitService.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Data.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly HabitDbContext _context;

        public HabitRepository(HabitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Habit>> GetPredefinedHabitsAsync()
        {
            return await _context.Habits
                .Where(h => h.CreatedBy == null)
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Habit?> GetByIdAsync(Guid id)
        {
            return await _context.Habits.FindAsync(id);
        }

        public async Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId)
        {
            return await _context.Habits
                .Where(h => h.CreatedBy == userId)
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Habit> AddAsync(Habit habit)
        {
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
            return habit;
        }

        public async Task UpdateAsync(Habit habit)
        {
            _context.Habits.Update(habit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Habit habit)
        {
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Habits.AnyAsync(h => h.Id == id);
        }
    }
}
