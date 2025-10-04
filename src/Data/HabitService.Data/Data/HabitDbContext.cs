using HabitService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HabitService.Data.Data
{
    public class HabitDbContext : DbContext
    {
        public HabitDbContext(DbContextOptions<HabitDbContext> options) : base(options) { }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<UserHabit> UserHabits { get; set; }
        public DbSet<HabitCompletion> HabitCompletions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Habit configuration for PostgreSQL
            modelBuilder.Entity<Habit>(entity =>
            {
                entity.ToTable("habits");

                entity.HasKey(h => h.Id);

                entity.Property(h => h.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(h => h.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(h => h.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(h => h.PeriodInDays)
                    .IsRequired()
          
                    .HasColumnName("period_in_days");
                //?

                entity.Property(h => h.TargetValue)
                    .HasColumnName("target_value");



                entity.Property(h => h.CreatedBy)
                    .HasColumnName("created_by");

                entity.Property(h => h.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("NOW()");

                // Indexes

                entity.HasIndex(h => h.CreatedBy)
                    .HasDatabaseName("ix_habits_created_by");

            });

            // UserHabit configuration for PostgreSQL
            modelBuilder.Entity<UserHabit>(entity =>
            {
                entity.ToTable("user_habits");

                entity.HasKey(uh => uh.Id);

                entity.Property(uh => uh.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(uh => uh.UserId)
                    .HasColumnName("user_id");

                entity.Property(uh => uh.HabitId)
                    .HasColumnName("habit_id");

                entity.Property(uh => uh.CurrentValue)
                    .HasColumnName("current_value");

                entity.Property(uh => uh.StartDate)
                    .HasColumnName("start_date")
                    .HasDefaultValueSql("NOW()");

                entity.Property(uh => uh.EndDate)
                    .HasColumnName("end_date");

                entity.Property(uh => uh.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                // Relationships
                entity.HasOne(uh => uh.Habit)
                    .WithMany()
                    .HasForeignKey(uh => uh.HabitId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(uh => uh.UserId)
                    .HasDatabaseName("ix_user_habits_user_id");

                entity.HasIndex(uh => uh.HabitId)
                    .HasDatabaseName("ix_user_habits_habit_id");

                entity.HasIndex(uh => uh.IsActive)
                    .HasDatabaseName("ix_user_habits_is_active");

                entity.HasIndex(uh => new { uh.UserId, uh.HabitId })
                    .IsUnique()
                    .HasDatabaseName("ix_user_habits_user_id_habit_id_unique");
            });

            // HabitCompletion configuration for PostgreSQL
            modelBuilder.Entity<HabitCompletion>(entity =>
            {
                entity.ToTable("habit_completions");

                entity.HasKey(hc => hc.Id);

                entity.Property(hc => hc.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(hc => hc.UserHabitId)
                    .HasColumnName("user_habit_id");

                entity.Property(hc => hc.CompletedDate)
                    .HasColumnName("completed_date")
                    .HasDefaultValueSql("NOW()");

                entity.Property(hc => hc.CompletedValue)
                    .HasColumnName("completed_value");

                // Relationships
                entity.HasOne(hc => hc.UserHabit)
                    .WithMany(uh => uh.Completions)
                    .HasForeignKey(hc => hc.UserHabitId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(hc => hc.UserHabitId)
                    .HasDatabaseName("ix_habit_completions_user_habit_id");

                entity.HasIndex(hc => hc.CompletedDate)
                    .HasDatabaseName("ix_habit_completions_completed_date");
            });

            // Seed predefined habits
            modelBuilder.Entity<Habit>().HasData(GetPredefinedHabits());
        }

        private static List<Habit> GetPredefinedHabits()
        {
            return new List<Habit>
            {
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
