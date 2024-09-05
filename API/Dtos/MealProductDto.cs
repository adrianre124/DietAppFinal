using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MealProductDto
    {
        public int MealId { get; set; }
        public int ProductId { get; set; }
        public decimal Weight { get; set; } = 100;
    }
}