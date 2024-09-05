using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class DietPlanController(IUnitOfWork unitOfWork, IMapper mapper) : BaseController
    {
        [HttpGet("{dietPlanId}")]
        public async Task<ActionResult<DietPlanDto>> GetDietPlan(int dietPlanId)
        {
            var dietPlan = await unitOfWork.dietPlanRepository.GetDietPlanAsync(dietPlanId);

            if (dietPlan == null) return NotFound();

            var dietPlanDto = mapper.Map<DietPlanDto>(dietPlan);

            return Ok(dietPlanDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DietPlanDto>>> GetDietPlans()
        {
            var user = await unitOfWork.userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound("User not found");

            var dietPlans = await unitOfWork.dietPlanRepository.GetdietPlansByUserIdAsync(user.Id);

            if (dietPlans == null) return NotFound("Diet Plans not found");

            var dietPlansDto = mapper.Map<IEnumerable<DietPlanDto>>(user.DietPlans);

            return Ok(dietPlansDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietPlan(CreateDietPlanDto createDietPlanDto)
        {
            var user = await unitOfWork.userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound("User not found");

            var dietPlan = new DietPlan
            {
                Name = createDietPlanDto.Name,
                CreateDate = DateTime.Now,
                UserId = user.Id
            };

            unitOfWork.dietPlanRepository.AddDietPlan(dietPlan);

            if (await unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(GetDietPlan), new { dietPlanId = dietPlan.DietPlanId }, mapper.Map<DietPlanDto>(dietPlan));
            }

            return BadRequest("There was a problem creating DietPlan");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDietPlan(int id, DietPlanDto dietPlanDto)
        {
            var dietPlan = await unitOfWork.dietPlanRepository.GetDietPlanAsync(id);

            if (dietPlan == null || !ModelState.IsValid)
            {
                return NotFound($"DietPlan with ID {id} not found.");
            }

            dietPlan.Name = dietPlanDto.Name;

            var updatedMealsIds = dietPlanDto.Meals.Select(m => m.MealId).ToList();
            var mealsToRemove = dietPlan.Meals.Where(m => !updatedMealsIds.Contains(m.MealId)).ToList();

            foreach(var meal in mealsToRemove)
            {
                unitOfWork.mealRepository.DeleteMeal(meal);
            }

            foreach (var mealDto in dietPlanDto.Meals)
            {
                Meal existingMeal;

                if (mealDto.MealId != 0)
                {
                    existingMeal = dietPlan.Meals.FirstOrDefault(m => m.MealId == mealDto.MealId)!;

                    if (existingMeal != null)
                    {
                        mapper.Map(mealDto, existingMeal);

                        existingMeal.MealProducts.Clear();
                    }
                    else
                    {
                        return BadRequest($"Meal with ID {mealDto.MealId} not found in this DietPlan.");
                    }
                }
                else
                {
                    existingMeal = mapper.Map<Meal>(mealDto);
                    dietPlan.Meals.Add(existingMeal);
                }

                foreach (var productDto in mealDto.Products)
                {
                    var product = await unitOfWork.productRepository.GetProductAsync(productDto.ProductId);

                    if (product == null)
                    {
                        return BadRequest($"Product with ID {productDto.ProductId} not found.");
                    }

                    existingMeal.MealProducts.Add(new MealProduct
                    {
                        Meal = existingMeal,
                        Product = product,
                        Weight = productDto.Weight
                    });
                }
            }

            unitOfWork.dietPlanRepository.UpdateDietPlan(dietPlan);

            if (await unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed to update diet plan.");
        }

        [HttpDelete("delete/{dietPlanId}")]
        public async Task<IActionResult> DeleteDietPlan(int dietPlanId)
        {
            var user = await unitOfWork.userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound("User not found");

            var dietPlan = await unitOfWork.dietPlanRepository.GetDietPlanAsync(dietPlanId);

            if (dietPlan == null || dietPlan.UserId != user.Id)
                return NotFound("Diet Plan not found or does not belong to user");

            unitOfWork.dietPlanRepository.DeleteDietPlan(dietPlan);

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleteing diet plan");
        }

        [HttpPost("{dietPlanId}/add-meal")]
        public async Task<IActionResult> AddMeal(int dietPlanId, CreateMealDto createMealDto)
        {
            var dietPlan = await unitOfWork.dietPlanRepository.GetDietPlanAsync(dietPlanId);

            if (dietPlan == null) return NotFound("Diet plan not found.");

            var meal = new Meal
            {
                Name = createMealDto.Name,
                DietPlanId = dietPlanId,
            };

            foreach (var productDto in createMealDto.Products)
            {
                var product = await unitOfWork.productRepository.GetProductAsync(productDto.ProductId);

                if (product == null) return NotFound("Product not found");

                var mealProduct = new MealProduct
                {
                    Meal = meal,
                    Product = product,
                    Weight = productDto.Weight
                };

                meal.MealProducts.Add(mealProduct);
            }

            unitOfWork.mealRepository.AddMeal(meal);

            if (await unitOfWork.Complete())
            {
                var mealDto = mapper.Map<MealDto>(meal);
                return Ok(mealDto);
            }

            return BadRequest("There was a problem adding meal to diet plan");
        }

        [HttpDelete("{dietPlanId}/delete-meal/{mealId}")]
        public async Task<IActionResult> DeleteMeal(int dietPlanId, int mealId)
        {
            var dietPlan = await unitOfWork.dietPlanRepository.GetDietPlanAsync(dietPlanId);
            if (dietPlan == null) return NotFound("Diet plan not found");

            var meal = await unitOfWork.mealRepository.GetMealAsync(mealId);
            if (meal == null || meal.DietPlanId != dietPlanId)
                return NotFound("Meal not found or does not belong to the specified diet plan.");

            unitOfWork.mealRepository.DeleteMeal(meal);

            if (await unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("There was a problem deleting the meal");
        }
    }
}