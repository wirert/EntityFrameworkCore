using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {           
            CreateMap<Product, ExportProductDto>()
                .ForMember(x => x.Seller, y => y.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            CreateMap<Product, ExportSoldProductDto>()
                .ForMember(d => d.BuyerFirstName, o => o.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, o => o.MapFrom(s => s.Buyer.LastName));
            CreateMap<User, ExportUserWithSoldProductDto>()
                .ForMember(d => d.SoldProducts, o => o.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            CreateMap<Category, ExportCategoryByProductCountDto>()
                .ForMember(d => d.ProductsCount, o => o.MapFrom(s => s.CategoriesProducts.Count()))
                .ForMember(d => d.AveragePrice, o => o.MapFrom(s => 
                                        s.CategoriesProducts.Average(cp => cp.Product.Price).ToString("f2")))
                .ForMember(d => d.TotalRevenue, o => o.MapFrom(s => 
                                        s.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("f2")));

            
        }
    }
}
