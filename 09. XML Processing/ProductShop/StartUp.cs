using System.Xml.Serialization;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Export.CategoriesByProducts;
using ProductShop.DTOs.Export.SoldProducts;
using ProductShop.DTOs.Export.UsersAndProducts;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            //CreateAndSeedDb(context);

            File.WriteAllText(GetExportPath("users-and-products.xml"), GetUsersWithProducts(context));
        }

        private static string GetExportPath(string fileName)
            => $"../../../Results/{fileName}";


        private static Mapper NewMapper()
            => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));

        private static string ReadTextFromFile(string fileName)
            => File.ReadAllText($"../../../Datasets/{fileName}");

        private static void CreateAndSeedDb(ProductShopContext context)
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

        //05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, null);

            var serializer = new XmlSerializer(typeof(ProductInRangeDto[]), new XmlRootAttribute("Products"));

            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Include(p => p.Buyer)
                .Select(p => new ProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerFullName = $"{p.Buyer.FirstName} " + p.Buyer.LastName
                })
                .ToArray();

            using var writer = new StringWriter();

            serializer.Serialize(writer, productsInRange, namespaces);

            return writer.ToString();
        }

        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(u => u.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProductsSold = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new SoldProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserSoldProductsDto[]), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, null);

            using var writer = new StringWriter();

            serializer.Serialize(writer, users, namespaces);

            return writer.ToString();
        }

        //07. Export Categories By Products Count 
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Include(c => c.CategoryProducts)
                .Select(c => new CategoryByProductsDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);

            var serializer = new XmlSerializer(typeof(CategoryByProductsDto[]), new XmlRootAttribute("Categories"));

            using var writer = new StringWriter();

            serializer.Serialize(writer, categories, namespaces);

            return writer.ToString();
        }

        //08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProducts = new UserCountDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = context.Users
                    .Where(u => u.ProductsSold.Any())
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Select(u => new UserProductsDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        ProductsSold = new SoldProductsDto
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(p => new ProductUserDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                        }
                    })
                    .Take(10)
                    .ToArray()
            };            

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);

            var serializer = new XmlSerializer(typeof(UserCountDto), new XmlRootAttribute("Users"));

            using var writer = new StringWriter();

            serializer.Serialize(writer, usersWithProducts, namespaces);

            return writer.ToString();
        }
    }
}