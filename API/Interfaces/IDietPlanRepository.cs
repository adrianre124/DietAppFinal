using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IDietPlanRepository
    {
        public void AddDietPlan(DietPlan dietPlan);
        public void UpdateDietPlan(DietPlan dietPlan);
        public void DeleteDietPlan(DietPlan dietPlan);
        public Task<DietPlan?> GetDietPlanAsync(int dietPlanId);
        public Task<IEnumerable<DietPlan>> GetdietPlansByUserIdAsync(Guid userId);
    }
}