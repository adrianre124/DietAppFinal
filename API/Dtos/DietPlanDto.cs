using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class DietPlanDto
    {
        public int DietPlanId { get; set; }
        public required string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<MealDto> Meals { get; set; } = [];
    }
}