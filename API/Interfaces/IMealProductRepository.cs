using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IMealProductRepository
    {
        public bool MealProductsExists(Product existingProduct);
        public void AddMealProducts(MealProduct mealProduct);
        public void RemoveMealProduct(MealProduct mealProduct);
        public void RemoveMealProducts(IEnumerable<MealProduct> mealProducts);
        public Task<IEnumerable<MealProduct>> GetMealProductsByProductIdAsync(int productId);
    }
}