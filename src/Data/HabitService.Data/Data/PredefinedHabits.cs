using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Data.Data
{
    public static class PredefinedHabits
    {
        public static List<Habit> GetPredefinedHabits()
        {
            return new List<Habit>{
                new Habit
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Пить 2 литра воды в день",
                    Description = "Поддержание водного баланса организма",
                    PeriodInDays = 1,
                    TargetValue = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Читать 30 минут",
                    Description = "Ежедневное чтение для саморазвития",
                    PeriodInDays = 1,
                    TargetValue = 30,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Заниматься спортом 3 раза в неделю",
                    Description = "Регулярные физические нагрузки",
                    PeriodInDays = 7,
                    TargetValue = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Медитировать 10 минут",
                    Description = "Ежедневная практика mindfulness",
                    PeriodInDays = 1,
                    TargetValue = 10,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}