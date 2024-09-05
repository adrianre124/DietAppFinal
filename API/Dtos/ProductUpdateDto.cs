using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Dtos
{
    public class ProductUpdateDto
    {
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
    }
}