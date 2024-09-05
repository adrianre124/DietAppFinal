using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class DietPlan
    {
        public int DietPlanId { get; set; }
        public required string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<Meal> Meals { get; set; } = [];
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;

    }
}