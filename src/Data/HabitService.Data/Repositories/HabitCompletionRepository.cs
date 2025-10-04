using HabitService.Business.Interfaces.IRepositories;
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
    public class HabitCompletionRepository : IHabitCompletionRepository
    {
        private readonly HabitDbContext _context;

        public HabitCompletionRepository(HabitDbContext context)
        {
            _context = context;
        }

        public async Task<HabitCompletion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.HabitCompletions
                .Include(hc => hc.UserHabit)
                .ThenInclude(uh => uh.Habit)
                .FirstOrDefaultAsync(hc => hc.Id == id, cancellationToken);
        }

        public async Task<List<HabitCompletion>> GetByUserHabitIdAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            return await _context.HabitCompletions
                .Where(hc => hc.UserHabitId == userHabitId)
                .OrderByDescending(hc => hc.CompletedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<HabitCompletion>> GetByUserHabitIdAndDateRangeAsync(Guid userHabitId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.HabitCompletions
                .Where(hc => hc.UserHabitId == userHabitId &&
                           hc.CompletedAt >= startDate &&
                           hc.CompletedAt <= endDate)
                .OrderBy(hc => hc.CompletedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<HabitCompletion> AddAsync(HabitCompletion completion, CancellationToken cancellationToken = default)
        {
            _context.HabitCompletions.Add(completion);
            await _context.SaveChangesAsync(cancellationToken);
            return completion;
        }

        public async Task UpdateAsync(HabitCompletion completion, CancellationToken cancellationToken = default)
        {
            _context.HabitCompletions.Update(completion);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(HabitCompletion completion, CancellationToken cancellationToken = default)
        {
            _context.HabitCompletions.Remove(completion);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetCompletionsCountForPeriodAsync(Guid userHabitId, DateTime periodStart, CancellationToken cancellationToken = default)
        {
            return await _context.HabitCompletions
                .Where(hc => hc.UserHabitId == userHabitId && hc.CompletedAt >= periodStart)
                .SumAsync(hc => hc.Value, cancellationToken);
        }

        public async Task<bool> HasCompletionTodayAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _context.HabitCompletions
                .AnyAsync(hc => hc.UserHabitId == userHabitId &&
                              hc.CompletedAt >= today &&
                              hc.CompletedAt < tomorrow, cancellationToken);
        }
    }
}
