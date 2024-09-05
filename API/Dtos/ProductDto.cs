using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Calories { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Proteins { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Carbohydrates { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Fats { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Salt { get; set; }
        [ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public decimal? Sugars { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Weight { get; set; }
    }
}