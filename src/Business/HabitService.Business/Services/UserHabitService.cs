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
    public class UserHabitService: IUserHabitService
    {
        private readonly IUserHabitRepository _userHabitRepository;
        private readonly IHabitRepository _habitRepository;

        public UserHabitService(IUserHabitRepository userHabitRepository, IHabitRepository habitRepository)
        {
            _userHabitRepository = userHabitRepository;
            _habitRepository = habitRepository;
        }

        public async Task<List<UserHabit>> GetUserHabitsAsync(Guid userId)
        {
            return await _userHabitRepository.GetByUserIdAsync(userId);
        }

        public async Task<UserHabit> AddHabitToUserAsync(Guid userId, Guid habitId)
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
                CurrentValue = 0,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _userHabitRepository.AddAsync(userHabit);
        }

        public async Task<UserHabit> UpdateProgressAsync(Guid userHabitId, int newValue)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId);
            if (userHabit == null)
                throw new Exception($"User habit with ID {userHabitId} not found");

            if (!userHabit.IsActive)
                throw new InvalidOperationException("Cannot update progress for inactive habit");

            if (newValue < 0)
                throw new ArgumentException("Progress value cannot be negative");

            userHabit.CurrentValue = newValue;


            await _userHabitRepository.UpdateAsync(userHabit);
            return userHabit;
        }

        public async Task CompleteHabitAsync(Guid userHabitId)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId);
            if (userHabit == null)
                throw new Exception($"User habit with ID {userHabitId} not found");

            userHabit.IsActive = false;
            userHabit.EndDate = DateTime.UtcNow;

            await _userHabitRepository.UpdateAsync(userHabit);
        }

        public async Task RemoveHabitFromUserAsync(Guid userHabitId)
        {
            var userHabit = await _userHabitRepository.GetByIdAsync(userHabitId);
            if (userHabit == null)
                throw new Exception($"User habit with ID {userHabitId} not found");

            await _userHabitRepository.DeleteAsync(userHabit);
        }

        public async Task<UserHabit?> GetUserHabitByIdAsync(Guid userHabitId)
        {
            return await _userHabitRepository.GetByIdAsync(userHabitId);
        }
    }
}
