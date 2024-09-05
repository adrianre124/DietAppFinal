using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using API.Data;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository userRepository { get; }
        public IDietPlanRepository dietPlanRepository { get; }
        public IMealRepository mealRepository { get; }
        public IProductRepository productRepository{ get; }
        public IMealProductRepository mealProductRepository { get; }
        public DataContext Context{ get; }
        Task<bool> Complete();
        bool HasChange();
    }
}