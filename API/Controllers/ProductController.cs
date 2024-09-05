using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class ProductController(HttpClient httpClient, IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService) : BaseController
    {
        [Authorize]
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int productId)
        {
            var product = await unitOfWork.productRepository.GetProductAsync(productId);

            if (product == null) return NotFound();

            return mapper.Map<ProductDto>(product);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetUserProducts()
        {
            var user = await unitOfWork.userRepository.GetUserByIdAsync(User.GetUserId());

            if (user == null) return NotFound("User not found");

            var products = await unitOfWork.productRepository.GetAllProductsByUserIdAsync(user.Id);

            if (products == null) return NotFound("Products not found");

            return Ok(mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> SearchProducts([FromQuery] string productName)
        {
            const int searchLimit = 10;

            if (string.IsNullOrWhiteSpace(productName)) return BadRequest("Product name cannot be empty.");

            List<Product> products = [];

            if (User.Identity?.IsAuthenticated != false)
            {
                var user = await unitOfWork.userRepository.GetUserByUsernameAsync(User.GetUsername());
                
                if (user == null) return NotFound("User not found");
                
                var userProducts = await unitOfWork.productRepository.GetProductsByUserIdAndProductNameAsync(user.Id, productName);
                products = products.Concat(userProducts).ToList();
            }
            
            if (products.Count >= searchLimit)
            {
                return mapper.Map<List<ProductDto>>(products);  
            }

            var productsFromDb = await unitOfWork.productRepository.SearchProductByNameAsync(productName);

            products = products.Concat(productsFromDb.Take(searchLimit - products.Count)).ToList();

            if (products.Count >= searchLimit)
            {
                return mapper.Map<List<ProductDto>>(products);
            }

            var productsResponse = "";

            using (httpClient)
            {
                httpClient.BaseAddress = new Uri("https://world.openfoodfacts.org/cgi/");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "DietApp - WebAPI - Version 1.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync($"search.pl?search_terms={productName}&fields=product_name,energy-kcal_100g,fat_100g,carbohydrates_100g,sugars_100g,proteins_100g,salt_100g,image_url&search_simple=1&json=true");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode) 
                {
                    return mapper.Map<List<ProductDto>>(products);
                }
                productsResponse = await response.Content.ReadAsStringAsync();
            }

            var jsonResponse = JsonNode.Parse(productsResponse);
            var productsArray = jsonResponse?["products"]?.AsArray();

            if (productsArray == null || productsArray.Count == 0)
                return NotFound("No products found.");

            var productNames = new List<Product>();

            foreach (var product in productsArray)
            {
                var nutritionFacts = new Product
                {
                    ProductName = product?["product_name"]?.ToString()!,
                    Calories = (decimal?)(product?["energy-kcal_100g"]),
                    Fats = (decimal?)(product?["fat_100g"]),
                    Carbohydrates = (decimal?)(product?["carbohydrates_100g"]),
                    Sugars = (decimal?)(product?["sugars_100g"]),
                    Proteins = (decimal?)(product?["proteins_100g"]),
                    Salt = (decimal?)(product?["salt_100g"]),
                    ImageUrl = product?["image_url"]?.ToString()
                };

                if (!unitOfWork.productRepository.Exists(nutritionFacts.ImageUrl!)) 
                {
                     var returnedProduct = unitOfWork.productRepository.AddAndReturnProduct(nutritionFacts);
                     nutritionFacts.ProductId = returnedProduct.ProductId;
                     if (productNames.Count <= searchLimit - products.Count)
                     {
                        productNames.Add(nutritionFacts);
                     }
                }
            }

            if (productNames.Count == 0) {
                if (products.Count > 0) {
                    return Ok(mapper.Map<List<ProductDto>>(products));
                }

                return NotFound("No valid products found.");
            }

            products.AddRange(productNames);

            return mapper.Map<List<ProductDto>>(products);
        }

        [Authorize]
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await unitOfWork.userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound("User not found");

            var product = mapper.Map<Product>(productDto);
            product.UserId = user.Id;

            if (image != null)
            {
                var uploadResult = await photoService.AddImageAsync(image);

                if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

                product.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                product.ImagePublicId = uploadResult.PublicId;
            }

            unitOfWork.productRepository.AddProduct(product);

            if (await unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(GetProduct), new { productId = product.ProductId }, mapper.Map<ProductDto>(product));
            }

            return BadRequest("Problem adding product");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productUpdateDto, IFormFile? image)
        {
            var product = await unitOfWork.productRepository.GetProductAsync(id);

            if (product == null) return NotFound("Product not found");

            mapper.Map(productUpdateDto, product);

            if (image != null)
            {
                var photoId = product.ImagePublicId;
                if (photoId != null)
                {
                    var deletionResult = await photoService.DeleteImageAsync(photoId);
                    if (deletionResult.Error != null) return BadRequest(deletionResult.Error.Message);
                }
                
                var uploadResult = await photoService.AddImageAsync(image);

                if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

                product.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                product.ImagePublicId = uploadResult.PublicId;
            }

            if (await unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed updating product");
            
        }

        [Authorize]
        [HttpDelete("delete-photo/{productId}")]
        public async Task<ActionResult> DeletePhoto(int productId)
        {
            var product = await unitOfWork.productRepository.GetProductAsync(productId);

            if (product == null) return NotFound("Product not found");

            var photoId = product.ImagePublicId;

            if (photoId != null)
            {
                var deletionResult = await photoService.DeleteImageAsync(photoId);
                if (deletionResult.Error != null) return BadRequest(deletionResult.Error.Message);

                product.ImageUrl = null;
                product.ImagePublicId = null;

                unitOfWork.productRepository.UpdateProduct(product);

                if (await unitOfWork.Complete()) return Ok();
            }

            return BadRequest("Problem deleteing image");
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            using (var transaction = await unitOfWork.Context.Database.BeginTransactionAsync())
            {
                var product = await unitOfWork.productRepository.GetProductAsync(productId);

                if (product == null) return NotFound("Product not found");

                if (product.ImageUrl != null)
                {
                    var deletionResult = await photoService.DeleteImageAsync(product.ImagePublicId!);
                    if (deletionResult.Error != null) return BadRequest(deletionResult.Error.Message);
                }

                var mealProducts = await unitOfWork.mealProductRepository.GetMealProductsByProductIdAsync(productId);
                unitOfWork.mealProductRepository.RemoveMealProducts(mealProducts);

                unitOfWork.productRepository.DeleteProduct(product);

                if (await unitOfWork.Complete())
                {
                    await transaction.CommitAsync();
                    return NoContent();
                }

                await transaction.RollbackAsync();
                return BadRequest("Failed to delete product");
            }
        }

        private decimal? ScaleNutrient(decimal? nutrientValuePer100g, decimal weight)
        {
            if (nutrientValuePer100g == null || nutrientValuePer100g.Value == 0) return 0;

            return nutrientValuePer100g.Value * (weight / 100);
        }

        private Product ScaleProduct(Product product, decimal weight)
        {
            product.Calories = ScaleNutrient(product.Calories, weight);
            product.Proteins = ScaleNutrient(product.Proteins, weight);
            product.Carbohydrates = ScaleNutrient(product.Carbohydrates, weight);
            product.Sugars = ScaleNutrient(product.Sugars, weight);
            product.Fats = ScaleNutrient(product.Fats, weight);
            product.Sugars = ScaleNutrient(product.Sugars, weight);

            return product;
        }
    }
}