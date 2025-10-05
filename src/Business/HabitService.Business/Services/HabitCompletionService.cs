using HabitService.Business.Interfaces.IRepositories;
using HabitService.Business.Interfaces.IServices;
using HabitService.Business.Interfaces.Repositories;
using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Services
{
    public class HabitCompletionService : IHabitCompletionService
    {
        private readonly IHabitCompletionRepository _completionRepository;
        private readonly IUserHabitRepository _userHabitRepository;

        public HabitCompletionService(
            IHabitCompletionRepository completionRepository,
            IUserHabitRepository userHabitRepository)
        {
            _completionRepository = completionRepository;
            _userHabitRepository = userHabitRepository;
        }

        public async Task<HabitCompletion> CompleteHabitAsync(Guid userHabitId, int value = 1, string? notes = null, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId, cancellationToken);
            if (userHabit == null || !userHabit.IsActive)
                throw new InvalidOperationException("User habit not found or not active");

            await ResetIfPeriodEndedAsync(userHabit, cancellationToken);

            var progress = await GetCurrentProgressAsync(userHabitId, cancellationToken);

            if (progress.CurrentValue + value > userHabit.Habit.TargetValue)
            {
                throw new InvalidOperationException($"Cannot complete habit. Current: {progress.CurrentValue}/{userHabit.Habit.TargetValue}");
            }

            var completion = new HabitCompletion
            {
                Id = Guid.NewGuid(),
                UserHabitId = userHabitId,
                CompletedAt = DateTime.UtcNow,
                Value = value,
                Notes = notes
            };

            return await _completionRepository.AddAsync(completion, cancellationToken);
        }

        public async Task<List<HabitCompletion>> GetCompletionsAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            return await _completionRepository.GetByUserHabitIdAsync(userHabitId, cancellationToken);
        }

        public async Task<HabitProgress> GetCurrentProgressAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId, cancellationToken);
            if (userHabit == null)
                throw new InvalidOperationException("User habit not found");

            var originalStartDate = userHabit.StartDate;

            var wasReset = await ResetIfPeriodEndedAsync(userHabit, cancellationToken);

            var (periodStart, periodEnd) = GetCurrentPeriod(new UserHabit
            {
                StartDate = originalStartDate,
                Habit = userHabit.Habit
            });

            var completionsCount = await _completionRepository.GetCompletionsCountForPeriodAsync(userHabitId, periodStart, cancellationToken);

            return new HabitProgress
            {
                CurrentValue = completionsCount,
                TargetValue = userHabit.Habit.TargetValue,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            };
        }

        public async Task<bool> CanCompleteHabitTodayAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            return !await _completionRepository.HasCompletionTodayAsync(userHabitId, cancellationToken);
        }

        public async Task<bool> ResetIfPeriodEndedAsync(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            var (_, periodEnd) = GetCurrentPeriod(userHabit);

            if (DateTime.UtcNow >= periodEnd)
            {
                await ResetHabitProgressAsync(userHabit, cancellationToken);
                return true;
            }

            return false;
        }

        public async Task ResetHabitProgressAsync(UserHabit userHabit, CancellationToken cancellationToken = default)
        {
            userHabit.StartDate = DateTime.UtcNow;
            await _userHabitRepository.UpdateAsync(userHabit, cancellationToken);
        }

        private (DateTime periodStart, DateTime periodEnd) GetCurrentPeriod(UserHabit userHabit)
        {
            if (userHabit.Habit.PeriodInDays <= 0)
                throw new InvalidOperationException("PeriodInDays must be positive");

            var elapsedTime = DateTime.UtcNow - userHabit.StartDate;

            var completedPeriods = (int)(elapsedTime.TotalDays / userHabit.Habit.PeriodInDays);

            var periodStart = userHabit.StartDate.AddDays(completedPeriods * userHabit.Habit.PeriodInDays);


            var periodEnd = periodStart.AddDays(userHabit.Habit.PeriodInDays);

            return (periodStart, periodEnd);
        }

        public async Task DeleteCompletionAsync(Guid completionId, CancellationToken cancellationToken = default)
        {
            var completion = await _completionRepository.GetByIdAsync(completionId, cancellationToken);
            if (completion == null)
                throw new InvalidOperationException("Completion record not found");

            await _completionRepository.DeleteAsync(completion, cancellationToken);
        }
    }
}
