using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CreateMealDto
    {
        public required string Name { get; set; }
        public List<MealProductDto> Products { get; set; } = [];
    }
}