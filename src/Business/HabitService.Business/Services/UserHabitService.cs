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
    public class UserHabitService : IUserHabitService
    {
        private readonly IUserHabitRepository _userHabitRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitCompletionService _completionService;

        public UserHabitService(
            IUserHabitRepository userHabitRepository,
            IHabitRepository habitRepository,
            IHabitCompletionService completionService)
        {
            _userHabitRepository = userHabitRepository;
            _habitRepository = habitRepository;
            _completionService = completionService;
        }

        public async Task<List<UserHabit>> GetUserHabitsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var habits = await _userHabitRepository.GetByUserIdAsync(userId, cancellationToken);

            foreach (var userHabit in habits)
            {
                await _completionService.ResetIfPeriodEndedAsync(userHabit, cancellationToken);
            }

            return habits;
        }

        public async Task<UserHabit> AddHabitToUserAsync(Guid userId, Guid habitId, CancellationToken cancellationToken = default)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null)
                throw new Exception($"Habit with ID {habitId} not found");

            var existingUserHabit = await _userHabitRepository.GetByUserAndHabitIdAsync(userId, habitId);
            if (existingUserHabit != null)
                throw new Exception($"User {userId} already has habit {habitId}");

            var userHabit = new UserHabit
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HabitId = habitId,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _userHabitRepository.AddAsync(userHabit, cancellationToken);
        }

        public async Task RemoveHabitFromUserAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId);
            if (userHabit == null)
                throw new Exception($"User habit with ID {userHabitId} not found");

            await _userHabitRepository.DeleteAsync(userHabit, cancellationToken);
        }

        public async Task<UserHabit?> GetUserHabitByIdAsync(Guid userHabitId, CancellationToken cancellationToken = default)
        {
            return await _userHabitRepository.GetByIdAsync(userHabitId, cancellationToken);
        }
    }
}
