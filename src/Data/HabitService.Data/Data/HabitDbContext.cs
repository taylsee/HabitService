using HabitService.Business.Models;
using Microsoft.EntityFrameworkCore;
using HabitService.Data.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
            modelBuilder.ApplyConfiguration(new HabitConfiguration());
            modelBuilder.ApplyConfiguration(new UserHabitConfiguration());
            modelBuilder.ApplyConfiguration(new HabitCompletionConfiguration());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Habit>().HasData(PredefinedHabits.GetPredefinedHabits());
        }
    }
}
