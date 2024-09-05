using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;

namespace API.Interfaces
{
    public interface IMealRepository
    {
        public void AddMeal(Meal meal);
        public void UpdateMeal(Meal meal);
        public void DeleteMeal(Meal meal);
        public Task<Meal?> GetMealAsync(int mealId);
        public Task<Meal?> GetMealOnlyAsync(int mealId);
    }
}