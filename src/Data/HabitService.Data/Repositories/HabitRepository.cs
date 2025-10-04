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

        public async Task<List<Habit>> GetPredefinedHabitsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Habits
                .Where(h => h.CreatedBy == null)
                .OrderBy(h => h.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Habit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Habits.FindAsync(id, cancellationToken);
        }

        public async Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Habits
                .Where(h => h.CreatedBy == userId)
                .OrderBy(h => h.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Habit> AddAsync(Habit habit, CancellationToken cancellationToken = default)
        {
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync(cancellationToken);
            return habit;
        }

        public async Task UpdateAsync(Habit habit, CancellationToken cancellationToken = default)
        {
            _context.Habits.Update(habit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Habit habit, CancellationToken cancellationToken = default)
        {
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Habits.AnyAsync(h => h.Id == id, cancellationToken);
        }
    }
}
