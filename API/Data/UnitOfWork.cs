using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork(DataContext context, IUserRepository UserRepository, 
        IProductRepository ProductRepository, IDietPlanRepository DietPlanRepository, 
        IMealRepository MealRepository, IMealProductRepository MealProductRepository) : IUnitOfWork
    {
        public IUserRepository userRepository => UserRepository;
        public IProductRepository productRepository => ProductRepository;
        public IDietPlanRepository dietPlanRepository => DietPlanRepository;
        public IMealRepository mealRepository => MealRepository;
        public IMealProductRepository mealProductRepository => MealProductRepository;
        public DataContext Context => context;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChange()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}