using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            CreateAndSeedDb(context);
        }

        private static void CreateAndSeedDb(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine(ImportSuppliers(context, GetJsonFromFile("suppliers.json")));
        }

        private static string GetJsonFromFile(string fileName)
                 => File.ReadAllText($"../../../Datasets/{fileName}");

        private static Mapper NewMapper()
            => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>()));

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);

            return isValid;
        }

        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var mapper = NewMapper();

            var suppliersDtos = JsonConvert.DeserializeObject<List<SupplierDto>>(inputJson);

            var suppliers = new List<Supplier>();

            foreach (var sDto in suppliersDtos)
            {
                suppliers.Add(mapper.Map<Supplier>(sDto));
            }

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

       


    }
}