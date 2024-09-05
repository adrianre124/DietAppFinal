using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class SearchProductDto
    {
        public required string ProductName { get; set; }
        public string? ImageUrl { get; set; }
    }
}