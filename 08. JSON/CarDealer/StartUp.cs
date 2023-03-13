using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Newtonsoft.Json;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

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
            Console.WriteLine(ImportParts(context, GetJsonFromFile("parts.json")));
            Console.WriteLine(ImportCars(context, GetJsonFromFile("cars.json")));
            Console.WriteLine(ImportCustomers(context, GetJsonFromFile("customers.json")));
            Console.WriteLine(ImportSales(context, GetJsonFromFile("sales.json")));
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

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var mapper = NewMapper();

            var partDtos = JsonConvert.DeserializeObject<List<PartDto>>(inputJson)
                .Where(p => context.Suppliers.Find(p.SupplierId) != null);

            //var parts = new List<Part>();
            //foreach (var pDto in partDtos)
            //{
            //    if (!IsValid(pDto))
            //    {
            //        continue;
            //    }

            //    parts.Add(mapper.Map<Part>(pDto));
            //}

            var parts = partDtos.Select(p => new Part
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                SupplierId = p.SupplierId
            })
                .ToList();

            context.AddRange(parts); 
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<List<CarDto>>(inputJson);

            var cars = carDtos.Select(c => new Car
            {
                Make = c.Make,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance,
                PartsCars = c.PartsId.Distinct().Select(i => new PartCar
                {
                    PartId = i
                })
                .ToArray()
            })
                .ToList();

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var mapper = NewMapper();

            var cDtos = JsonConvert.DeserializeObject<List<CustomerDto>>(inputJson);

            var customers = new List<Customer>();

            foreach (var cDto in cDtos)
            {
                customers.Add(mapper.Map<Customer>(cDto));
            }

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var saleDtos = JsonConvert.DeserializeObject<List<SaleDto>>(inputJson);

            var sales = saleDtos
                //.Where(s => context.Customers.Find( s.CustomerId) != null 
                //        && context.Cars.Find(s.CarId) != null)
                .Select(c => new Sale 
                {
                    CustomerId = c.CustomerId,
                    CarId = c.CarId,
                    Discount = c.Discount
                })
                .ToList();

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

    }
}