using HabitService.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Data.Data.Configurations
{
    internal class UserHabitConfiguration : IEntityTypeConfiguration<UserHabit>
    {
        public void Configure(EntityTypeBuilder<UserHabit> builder)
        {

            builder.ToTable("user_habits");

            builder.HasKey(uh => uh.Id);

            builder.Property(uh => uh.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(uh => uh.UserId)
                .HasColumnName("user_id");

            builder.Property(uh => uh.HabitId)
                .HasColumnName("habit_id");

            builder.Property(uh => uh.CurrentValue)
                .HasColumnName("current_value");

            builder.Property(uh => uh.StartDate)
                .HasColumnName("start_date")
                .HasDefaultValueSql("NOW()");

            builder.Property(uh => uh.EndDate)
                .HasColumnName("end_date");

            builder.Property(uh => uh.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.HasOne(uh => uh.Habit)
                .WithMany()
                .HasForeignKey(uh => uh.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(uh => uh.UserId)
                .HasDatabaseName("ix_user_habits_user_id");

            builder.HasIndex(uh => uh.HabitId)
                .HasDatabaseName("ix_user_habits_habit_id");

            builder.HasIndex(uh => uh.IsActive)
                .HasDatabaseName("ix_user_habits_is_active");

            builder.HasIndex(uh => new { uh.UserId, uh.HabitId })
                .IsUnique()
                .HasDatabaseName("ix_user_habits_user_id_habit_id_unique");

        }
    }
}
