using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        public void AddProduct(Product product);
        public Product AddAndReturnProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(Product product);
        public bool Exists(string imageUrl);
        public Task<Product?> GetProductAsync(int productId);
        public Task<Product?> GetProductByNameAsync(string productName);
        public Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(Guid userId);
        public Task<IEnumerable<Product>> GetProductsByUserIdAndProductNameAsync(Guid userID, string productName);
        public Task<IEnumerable<Product>> SearchProductByNameAsync(string partialName);
    }
}