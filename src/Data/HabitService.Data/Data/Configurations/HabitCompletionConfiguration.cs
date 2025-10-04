using HabitService.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Data.Data.Configurations
{
    internal class HabitCompletionConfiguration : IEntityTypeConfiguration<HabitCompletion>
    {
        public void Configure(EntityTypeBuilder<HabitCompletion> builder)
        {
            builder.HasKey(hc => hc.Id);

            builder.HasOne(hc => hc.UserHabit)
                .WithMany(uh => uh.Completions)
                .HasForeignKey(hc => hc.UserHabitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(hc => hc.CompletedAt)
                .IsRequired();

            builder.Property(hc => hc.Value)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
