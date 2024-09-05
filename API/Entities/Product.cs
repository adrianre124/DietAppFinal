using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public required string ProductName { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Calories mus be a positive number.")]
        public decimal? Calories { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Proteins mus be a positive number.")]
        public decimal? Proteins { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Carbohydrates mus be a positive number.")]
        public decimal? Carbohydrates { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Fats mus be a positive number.")]
        public decimal? Fats { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Salt mus be a positive number.")]
        public decimal? Salt { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Sugars mus be a positive number.")]
        public decimal? Sugars { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; } = [];
        public Guid? UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}