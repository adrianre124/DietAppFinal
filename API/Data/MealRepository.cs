using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MealRepository(DataContext context) : IMealRepository
    {
        public void AddMeal(Meal meal)
        {
            context.Meals.Add(meal);
        }

        public void DeleteMeal(Meal meal)
        {
            context.Meals.Remove(meal);
        }

        public async Task<Meal?> GetMealOnlyAsync(int mealId)
        {
            return await context.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);
        }

        public async Task<Meal?> GetMealAsync(int mealId)
        {
            return await context.Meals
                        .Include(m => m.MealProducts)
                        .ThenInclude(mp => mp.Product)
                        .FirstOrDefaultAsync(x => x.MealId == mealId);
        }

        public void UpdateMeal(Meal meal)
        {
            context.Meals.Update(meal);
        }
    }
}