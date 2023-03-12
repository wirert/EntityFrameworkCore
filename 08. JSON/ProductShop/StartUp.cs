using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using ProductShop.Data;
using ProductShop.Models;
using ProductShop.DTOs.Import;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var dbContext = new ProductShopContext();
            //string inputJson = GetJson("categories-products.json");

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine(ImportUsers(dbContext, File.ReadAllText("../../../Datasets/users.json")));
            //Console.WriteLine(ImportProducts(dbContext, File.ReadAllText("../../../Datasets/products.json")));
            //Console.WriteLine(ImportCategories(dbContext, File.ReadAllText("../../../Datasets/categories.json")));
            Console.WriteLine(ImportCategoryProducts(dbContext, File.ReadAllText("../../../Datasets/categories-products.json")));
        }

        private static string GetJson(string inputJsonFilename)
            => File.ReadAllText($"../../../Datasets/{inputJsonFilename}");

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);

            return isValid;
        }

        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<ImportUserDto>>(inputJson);

            var usersToAdd = users.Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age
            })
                .ToList();

            context.Users.AddRange(usersToAdd);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productsDtos = JsonConvert.DeserializeObject<List<ImportProductDto>>(inputJson);

            var products = productsDtos.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId
            })
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categoriesDtos = JsonConvert.DeserializeObject<List<ImportCategoryDto>>(inputJson, new JsonSerializerSettings()
            {
               NullValueHandling = NullValueHandling.Ignore
            });           

            var categories = categoriesDtos
                .Where(c => c.Name != null)
                .Select(c => new Category
            {
                Name = c.Name
            })
                .ToList();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var cpDtos = JsonConvert.DeserializeObject<List<ImportCategoryProductDto>>(inputJson);

            var categoryProducts = cpDtos
                .Where(cp => context.Products.Find(cp.ProductId) != null
                          && context.Categories.Find(cp.CategoryId) != null)
                .Select(cp => new CategoryProduct
            {
                CategoryId = cp.CategoryId,
                ProductId = cp.ProductId
            })
                .ToList();
            
            context.AddRange (categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }


    }
}