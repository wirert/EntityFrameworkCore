using System.ComponentModel.DataAnnotations;
using System.Globalization;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.DTOs.Export;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //CreateAndSeedDb(context);

            WriteJsonToFile("sales-discounts.json", GetSalesWithAppliedDiscount(context));
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

        private static void WriteJsonToFile(string fileName, string text)
                 => File.WriteAllText($"../../../Results/{fileName}", text);

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
                TraveledDistance = c.TraveledDistance,
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

        //14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var result = context.Customers
                .OrderBy(c => c.BirthDate.Date)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new OrderedCustomerDto
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        // 15. Export Cars From Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var result = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var result = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        //17. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carDtos = context.Cars
                .Select(c => new
                {
                    car = new CarWithPartsDto
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance,
                    },
                    parts = c.PartsCars.Select(p => new CarPartDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("F2")
                    })
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(carDtos, Formatting.Indented);
        }

        //18. Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var custumers = context.Customers
                .Where(c => c.Sales.Any())
                .Include(c => c.Sales)                
                .Select(c => new CustomerSalesDto
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(s => s.Car.PartsCars.Sum(p => p.Part.Price)).ToArray()
                })
                .ToArray();

            var result = custumers.Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.BoughtCars,
                spentMoney = c.SpentMoney.Sum()
            })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            return JsonConvert.SerializeObject (result, Formatting.Indented);
        }

        //19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //var sales = context.Sales
            //    .Select(s => new SaleDiscDto
            //    {
            //        CarInfo = new CarWithPartsDto
            //        {
            //            Make = s.Car.Make,
            //            Model = s.Car.Model,
            //            TraveledDistance = s.Car.TraveledDistance
            //        },
            //        CustomerName = s.Customer.Name,
            //        Discount = s.Discount,
            //        Price = s.Car.PartsCars.Sum(p => p.Part.Price)                    
            //    })
            //    .Take(10)
            //    .ToArray();

            var result = context.Sales
                .Select(s => new 
                {
                    car = new CarWithPartsDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Sum(p => p.Part.Price) * (100 - s.Discount)/100).ToString("f2")
                })
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}