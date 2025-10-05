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
    public class HabitCatalogService : IHabitCatalogService
    {
        private readonly IHabitRepository _habitRepository;

        public HabitCatalogService(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public async Task<List<Habit>> GetPredefinedHabitsAsync(CancellationToken cancellationToken = default)
        {
            return await _habitRepository.GetPredefinedHabitsAsync(cancellationToken);
        }

        public async Task<List<Habit>> GetUserCustomHabitsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _habitRepository.GetUserCustomHabitsAsync(userId, cancellationToken);
        }
        public async Task UpdateHabitAsync(Guid habitId, string name, string description,
            int periodInDays, int targetValue, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Habit name cannot be empty");

            if (targetValue <= 0)
                throw new ArgumentException("Target value must be positive");

            var habit = await _habitRepository.GetByIdAsync(habitId, cancellationToken);
            if (habit == null)
                throw new Exception($"Habit with ID {habitId} not found");

            habit.Name = name.Trim();
            habit.Description = description?.Trim() ?? "";
            habit.PeriodInDays = periodInDays;
            habit.TargetValue = targetValue;

            await _habitRepository.UpdateAsync(habit, cancellationToken);
        }

        public async Task<Habit> CreateCustomHabitAsync(Guid userId, string name, string description,
            int PeriodInDays, int targetValue, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Habit name cannot be empty");

            if (targetValue <= 0)
                throw new ArgumentException("Target value must be positive");

            var habit = new Habit
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description?.Trim() ?? "",
                PeriodInDays = PeriodInDays,
                TargetValue = targetValue,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            return await _habitRepository.AddAsync(habit, cancellationToken);
        }

        public async Task DeleteCustomHabitAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);

            if (habit == null)
                throw new Exception($"Habit with ID {habitId} not found");

            if (habit.CreatedBy != userId)
                throw new UnauthorizedAccessException("User can only delete their own custom habits");


            await _habitRepository.DeleteAsync(habit, cancellationToken);
        }

        public async Task<Habit?> GetHabitByIdAsync(Guid habitId, CancellationToken cancellationToken = default)
        {
            return await _habitRepository.GetByIdAsync(habitId, cancellationToken);
        }
    }
}
