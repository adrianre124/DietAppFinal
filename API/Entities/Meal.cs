using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;

namespace API.Entities
{
    public class Meal
    {
        public int MealId { get; set; }
        public required string Name { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; } = [];
        public int DietPlanId { get; set; }
        public DietPlan DietPlan { get; set; } = null!;
    }
}