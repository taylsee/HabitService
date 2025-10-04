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
    internal class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {

            builder.ToTable("habits");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            builder.Property(h => h.Description)
                .HasMaxLength(500)
                .HasColumnName("description");

            builder.Property(h => h.PeriodInDays)
                .IsRequired()
                .HasColumnName("period_in_days");

            builder.Property(h => h.TargetValue)
                .HasColumnName("target_value");

            builder.Property(h => h.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(h => h.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(h => h.CreatedBy)
                .HasDatabaseName("ix_habits_created_by");
        }
    }
}