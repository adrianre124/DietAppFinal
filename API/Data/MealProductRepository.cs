using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MealProductRepository(DataContext context) : IMealProductRepository
    {
        public void AddMealProducts(MealProduct mealProduct)
        {
            context.MealProducts.Add(mealProduct);
        }

        public void RemoveMealProduct(MealProduct mealProduct)
        {
            context.MealProducts.Remove(mealProduct);
        }

        public void RemoveMealProducts(IEnumerable<MealProduct> mealProducts)
        {
            context.MealProducts.RemoveRange(mealProducts);
        }

        public async Task<IEnumerable<MealProduct>> GetMealProductsByProductIdAsync(int productId)
        {
            return await context.MealProducts.Where(mp => mp.ProductId == productId).ToListAsync();
        }

        public bool MealProductsExists(Product existingProduct)
        {
            return context.MealProducts.Any(mp => mp.ProductId == existingProduct.ProductId);
        }
    }
}