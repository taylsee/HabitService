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
    public class UserHabitRepository : IUserHabitRepository
    {
        private readonly HabitDbContext _context;

        public UserHabitRepository(HabitDbContext context)
        {
            _context = context;
        }

        public async Task<UserHabit?> GetByIdAsync(Guid id)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .FirstOrDefaultAsync(uh => uh.Id == id);
        }

        public async Task<List<UserHabit>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .Where(uh => uh.UserId == userId && uh.IsActive)
                .OrderBy(uh => uh.StartDate)
                .ToListAsync();
        }

        public async Task<UserHabit?> GetByUserAndHabitIdAsync(Guid userId, Guid habitId)
        {
            return await _context.UserHabits
                .Include(uh => uh.Habit)
                .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HabitId == habitId && uh.IsActive);
        }

        public async Task<UserHabit> AddAsync(UserHabit userHabit)
        {
            _context.UserHabits.Add(userHabit);
            await _context.SaveChangesAsync();
            return userHabit;
        }

        public async Task UpdateAsync(UserHabit userHabit)
        {
            _context.UserHabits.Update(userHabit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserHabit userHabit)
        {
            _context.UserHabits.Remove(userHabit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserHasHabitAsync(Guid userId, Guid habitId)
        {
            return await _context.UserHabits
                .AnyAsync(uh => uh.UserId == userId && uh.HabitId == habitId && uh.IsActive);
        }
    }
}
