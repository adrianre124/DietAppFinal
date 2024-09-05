using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProductRepository(DataContext context) : IProductRepository
    {
        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public Product AddAndReturnProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public void UpdateProduct(Product product)
        {
            context.Products.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public bool Exists(string? imageUrl = "")
        {
            return context.Products.Any(p => p.ImageUrl == imageUrl);
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
        }

        public async Task<IEnumerable<Product>> SearchProductByNameAsync(string partialName)
        {
            return await context.Products
                                .Where(p => p.ProductName.ToLower().Contains(partialName.ToLower()))
                                .Where(p => p.UserId == null)
                                .Take(10)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(Guid userId)
        {
            return await context.Products
                                .Where(p => p.UserId == userId)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAndProductNameAsync(Guid userId, string productName)
        {
            return await context.Products
                                .Where(p => p.ProductName.ToLower().Contains(productName.ToLower()))
                                .Where(p => p.UserId == userId)
                                .Take(10)
                                .ToListAsync();
        }

    }
}