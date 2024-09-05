using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;

namespace API.Entities
{
    public class MealProduct
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Weight { get; set; } = 100;
    }
}