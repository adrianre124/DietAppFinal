using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, MemberDto>()
                .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()));
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, SearchProductDto>();
            CreateMap<DietPlanDto, DietPlan>()
                .EqualityComparison((dto, entity) => dto.DietPlanId == entity.DietPlanId);
            CreateMap<MealDto, Meal>()
                .EqualityComparison((dto, entity) => dto.MealId == entity.MealId);
            CreateMap<MealProductDto, MealProduct>()
                .EqualityComparison((dto, entity) => dto.MealId == entity.MealId && dto.ProductId == entity.ProductId);
            CreateMap<ProductDto, MealProduct>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new Product
                {
                    ProductId = src.ProductId > 0 ? src.ProductId : 0,
                    ProductName = src.ProductName!
                }))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));
            CreateMap<DietPlan, DietPlanDto>()
                .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.Meals));
            CreateMap<Meal, MealDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.MealProducts));
            CreateMap<MealProduct, ProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Product.Calories * src.Weight / (decimal)100.0))
                .ForMember(dest => dest.Proteins, opt => opt.MapFrom(src => src.Product.Proteins * src.Weight / (decimal)100.0))
                .ForMember(dest => dest.Carbohydrates, opt => opt.MapFrom(src => src.Product.Carbohydrates * src.Weight / (decimal)100.0))
                .ForMember(dest => dest.Sugars, opt => opt.MapFrom(src => src.Product.Sugars * src.Weight / (decimal)100.0))
                .ForMember(dest => dest.Fats, opt => opt.MapFrom(src => src.Product.Fats * src.Weight / (decimal)100.0))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Salt, opt => opt.MapFrom(src => src.Product.Salt * src.Weight / (decimal)100.0));
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        }
    }
}