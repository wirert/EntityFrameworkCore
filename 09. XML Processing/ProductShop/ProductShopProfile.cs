﻿using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>();
        }
    }
}
