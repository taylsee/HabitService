using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitService.Business.Models
{
    public class HabitCompletion
    {
        public Guid Id { get; set; }
        public Guid UserHabitId { get; set; }
        public UserHabit UserHabit { get; set; } = null!;
        public DateTime CompletedDate { get; set; } = DateTime.UtcNow;
        public int CompletedValue { get; set; }
    }
}
