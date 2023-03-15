using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            CreateAndSeedDb(context);
        }

        public static Mapper NewMapper()
            => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>())); 
        
        public static string ReadTextFromFile(string fileName)
            => File.ReadAllText($"../../../Datasets/{fileName}");

        public static void CreateAndSeedDb(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine(ImportUsers(context, ReadTextFromFile("users.xml")));
            Console.WriteLine(ImportProducts(context, ReadTextFromFile("products.xml")));
            Console.WriteLine(ImportCategories(context, ReadTextFromFile("categories.xml")));
            Console.WriteLine(ImportCategoryProducts(context, ReadTextFromFile("categories-products.xml")));
        }

        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
           XmlSerializer deserializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));

            using var reader = new StringReader(inputXml);

            var userDtos = (UserDto[])deserializer.Deserialize(reader);

            Mapper mapper = NewMapper();

            var users = mapper.Map<User[]>(userDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("Products"));

            using var reader = new StringReader(inputXml);

            var productDtos = (ProductDto[])serializer.Deserialize(reader);

            var mapper = NewMapper();

            var products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            using var reader = new StringReader(inputXml);

            var categoryDtos = (CategoryDto[])serializer.Deserialize(reader);

            var categories = new List<Category>();

            foreach (var cDto in categoryDtos)
            {
                categories.Add(new Category
                {
                    Name = cDto.Name
                });
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));

            using var reader = new StringReader(inputXml);

            var categoryProductDtos = (CategoryProductDto[])serializer.Deserialize(reader);

            var categoryProducts = new List<CategoryProduct>();

            foreach (var cpDto in categoryProductDtos)
            {
                if (context.Categories.Find(cpDto.CategoryId) == null || context.Products.Find(cpDto.ProductId) == null)
                {
                    continue;
                }

                categoryProducts.Add(new CategoryProduct
                {
                    CategoryId = cpDto.CategoryId,
                    ProductId = cpDto.ProductId
                });
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }



    }
}