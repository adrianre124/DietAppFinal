using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DietPlanRepository(DataContext context) : IDietPlanRepository
    {
        public void AddDietPlan(DietPlan dietPlan)
        {
            context.DietPlans.Add(dietPlan);
        }

        public void DeleteDietPlan(DietPlan dietPlan)
        {
            context.DietPlans.Remove(dietPlan);
        }

        public async Task<DietPlan?> GetDietPlanAsync(int dietPlanId)
        {
            return await context.DietPlans
                                .Include(dp => dp.Meals)
                                .ThenInclude(m => m.MealProducts)
                                .ThenInclude(mp => mp.Product)
                                .FirstOrDefaultAsync(dp => dp.DietPlanId == dietPlanId);
        }

        public async Task<IEnumerable<DietPlan>> GetdietPlansByUserIdAsync(Guid userId)
        {
            return await context.DietPlans
                                .Include(dp => dp.Meals)
                                .ThenInclude(m => m.MealProducts)
                                .ThenInclude(mp => mp.Product)
                                .Where(dp => dp.UserId == userId)
                                .OrderByDescending(dp => dp.CreateDate)
                                .ToListAsync(); 
        }

        public void UpdateDietPlan(DietPlan dietPlan)
        {
            context.Entry(dietPlan).State = EntityState.Modified;
        }
    }
}