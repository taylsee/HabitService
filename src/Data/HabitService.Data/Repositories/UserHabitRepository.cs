using HabitService.Business.Interfaces.Repositories;
using HabitService.Business.Models;
using HabitService.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HabitService.Data.Repositories
{
    public class UserHabitRepository : IUserHabitRepository
    {
        private readonly HabitDbContext _context;

        public UserHabitRepository(HabitDbContext context)
        {
            _context = context;
        }

        public async Task<UserHabit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .FirstOrDefaultAsync(uh => uh.Id == id, cancellationToken);
        }

        public async Task<List<UserHabit>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .Where(uh => uh.UserId == userId && uh.IsActive)
                .OrderBy(uh => uh.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserHabit?> GetByUserAndHabitIdAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HabitId == habitId && uh.IsActive, cancellationToken);
        }

        public async Task<UserHabit> AddAsync(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            _context.UserHabits.Add(userHabit);
            await _context.SaveChangesAsync(cancellationToken);
            return userHabit;
        }

        public async Task UpdateAsync(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            _context.UserHabits.Update(userHabit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            _context.UserHabits.Remove(userHabit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UserHasHabitAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            return await _context.UserHabits
                .AnyAsync(uh => uh.UserId == userId && uh.HabitId == habitId && uh.IsActive, cancellationToken);
        }
    }
}
