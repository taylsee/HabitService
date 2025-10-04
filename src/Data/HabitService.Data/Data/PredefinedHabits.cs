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
                    Name = "Пить 2 литра воды в день",
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
                    Name = "Читать 30 минут в день",
                    Description = "Ежедневное чтение для саморазвития",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Изучать английский 20 минут",
                    Description = "Регулярное изучение иностранного языка",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Слушать образовательный подкаст",
                    Description = "Развитие через аудиоконтент",
                    PeriodInDays = 7,
                    TargetValue = 2,
                    CreatedAt = DateTime.UtcNow
                },


                new Habit
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Медитировать 10 минут",
                    Description = "Ежедневная практика mindfulness",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Прогулка на свежем воздухе",
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
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Name = "Разбор почты 2 раза в день",
                    Description = "Организация входящих сообщений",
                    PeriodInDays = 1,
                    TargetValue = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                    Name = "Учет расходов",
                    Description = "Ежедневное отслеживание трат",
                    PeriodInDays = 1,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("23456789-2345-2345-2345-234567890123"),
                    Name = "Откладывать 10% от дохода",
                    Description = "Регулярные накопления",
                    PeriodInDays = 30,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Habit
                {
                    Id = Guid.Parse("45678901-4567-4567-4567-456789012345"),
                    Name = "Встреча с друзьями",
                    Description = "Социальная активность",
                    PeriodInDays = 7,
                    TargetValue = 1,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}