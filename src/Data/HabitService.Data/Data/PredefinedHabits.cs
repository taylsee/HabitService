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
            return new List<Habit>
            {
                new Habit
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Пить 2 литра воды в день (по 250мл)",
                    Description = "Поддержание водного баланса организма",
                    PeriodInDays = 1, 
                    TargetValue = 8,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Утренняя зарядка 15 минут",
                    Description = "Ежедневная физическая активность для бодрости",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Спорт 3 раза в неделю",
                    Description = "Регулярные интенсивные тренировки",
                    PeriodInDays = 7,
                    TargetValue = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Чистить зубы 2 раза в день",
                    Description = "Поддержание гигиены полости рта",
                    PeriodInDays = 1,
                    TargetValue = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Читать по 30 минут в день",
                    Description = "Ежедневное чтение для саморазвития",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Изучать английский по 20 минут в день",
                    Description = "Регулярное изучение иностранного языка",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Медитировать по 10 минут в день",
                    Description = "Ежедневная практика mindfulness",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Прогуливаться на свежем воздухе каждый день",
                    Description = "Ежедневные прогулки для снятия стресса",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Планировать день с вечера",
                    Description = "Составление плана на следующий день",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("23456789-2345-2345-2345-234567890123"),
                    Name = "Откладывать 10% от дохода каждый месяц",
                    Description = "Регулярные накопления",
                    PeriodInDays = 30,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}