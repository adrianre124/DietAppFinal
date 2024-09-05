using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MealDto
    {
        public int MealId { get; set; }
        public required string Name { get; set; }
        public ICollection<ProductDto> Products { get; set; } = [];
    }
}