using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs.Export.BMWCars;
using CarDealer.DTOs.Export.CarsWithDistance;
using CarDealer.DTOs.Export.CarsWithParts;
using CarDealer.DTOs.Export.LocalSuppliers;
using CarDealer.DTOs.Export.SalesWithDiscounts;
using CarDealer.DTOs.Export.TotalSalesByCustomer;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //ResetAndSeedDb(context);

            string resultXml = GetSalesWithAppliedDiscount(context);

            WriteXmlToFile(resultXml, "sales-discounts.xml");
        }

        private static void ResetAndSeedDb(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine(ImportSuppliers(context, ReadXml("suppliers.xml")));
            Console.WriteLine(ImportParts(context, ReadXml("parts.xml")));
            Console.WriteLine(ImportCars(context, ReadXml("cars.xml")));
            Console.WriteLine(ImportCustomers(context, ReadXml("customers.xml")));
            Console.WriteLine(ImportSales(context, ReadXml("sales.xml")));

        }

        private static string ReadXml(string fileName)
            => File.ReadAllText($"../../../Datasets/{fileName}");

        private static void WriteXmlToFile(string xml, string fileName)
            => File.WriteAllText($"../../../Results/{fileName}", xml);

        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));

            using var reader = new StringReader(inputXml);

            var supplierDtos = (ImportSupplierDto[])serializer.Deserialize(reader);
            var suppliers = new List<Supplier>();

            foreach (var sDto in supplierDtos)
            {
                suppliers.Add(new Supplier
                {
                    Name = sDto.Name,
                    IsImporter = sDto.IsImporter
                });
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            using var reader = new StringReader(inputXml);
            var partDtos = (ImportPartDto[])serializer.Deserialize(reader);
            var parts = new List<Part>();

            foreach (var pDto in partDtos)
            {
                if (context.Suppliers.Find(pDto.SupplierId) == null)
                {
                    continue;
                }

                parts.Add(new Part
                {
                    Name = pDto.Name,
                    Price = pDto.Price,
                    Quantity = pDto.Quantity,
                    SupplierId = pDto.SupplierId
                });
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            using var reader = new StringReader(inputXml);
            var carDtos = (ImportCarDto[])serializer.Deserialize(reader);

            var cars = carDtos
                .Select(c => new Car
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    PartsCars = c.PartsCars
                    .Select(p => p.PartId)
                    .Distinct()
                    .Select(i => new PartCar
                    {
                        PartId = i
                    })
                   .ToList()
                })
                .ToList();


            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            using var reader = new StringReader(inputXml);

            var customerDtos = (ImportCustomerDto[])serializer.Deserialize(reader);

            var customers = customerDtos
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Count}";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            using var reader = new StringReader(inputXml);

            var saleDtos = (ImportSaleDto[])serializer.Deserialize(reader);
            var carIds = context.Cars.Select(c => c.Id).ToList();
            var sales = saleDtos
                .Where(s => carIds.Contains(s.CarId))
                .Select(s => new Sale
                {
                    Discount = s.Discount,
                    CarId = s.CarId,
                    CustomerId = s.CustomerId
                })
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();


            return $"Successfully imported {sales.Count}";
        }

        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var carDtos = cars
                .Select(c => new CarWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();


            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);

            var serializer = new XmlSerializer(typeof(CarWithDistanceDto[]), new XmlRootAttribute("cars"));
            using var writer = new StringWriter();
            serializer.Serialize(writer, carDtos, namespaces);

            return writer.ToString();
        }

        //15. Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new BmwCarDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);
            var serializer = new XmlSerializer(typeof(BmwCarDto[]), new XmlRootAttribute("cars"));
            using var writer = new StringWriter();

            serializer.Serialize(writer, bmwCars, namespaces);

            return writer.ToString();
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new LocalSupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Parts = s.Parts.Count
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);
            var serializer = new XmlSerializer(typeof(LocalSupplierDto[]), new XmlRootAttribute("suppliers"));
            using var writer = new StringWriter();
            serializer.Serialize(writer, localSuppliers, namespaces);


            return writer.ToString();
        }

        //17. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Include(c => c.PartsCars)
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .Select(c => new CarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    PartsCars = c.PartsCars.Select(p => new CarPartDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);
            var serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));

            using var writer = new StringWriter();
            serializer.Serialize(writer, cars, namespaces);

            return writer.ToString();
        }

        //18. Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersDb = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    Sales = c.Sales.Select(s => new
                    {
                        Price = c.IsYoungDriver
                        ? s.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2))
                        : s.Car.PartsCars.Sum(p => (double)p.Part.Price)
                    })
                    .ToArray()
                })
                .ToArray();

            var customers = customersDb
                .OrderByDescending(c => c.Sales.Sum(s => s.Price))
                .Select(c => new CustomerSalesDto
                {
                    Name = c.Name,
                    BoughtCars = c.BoughtCars,
                    SpentMoney = c.Sales.Sum(s => s.Price).ToString("f2")
                })
                .ToArray();


            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);
            var serializer = new XmlSerializer(typeof(CustomerSalesDto[]), new XmlRootAttribute("customers"));
            using var writer = new StringWriter();
            serializer.Serialize(writer, customers, namespaces);

            return writer.ToString();
        }

        //19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Include(s => s.Car)
                .ThenInclude(c => c.PartsCars)
                .ThenInclude(pc => pc.Part)
                .Select(s => new SaleDto
                {
                    Car = new CarSaleDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("f2"),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars
                    .Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
                    .ToString()
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, null);
            var serializer = new XmlSerializer(typeof(SaleDto[]), new XmlRootAttribute("sales"));
            using var writer = new StringWriter();
            serializer.Serialize(writer, sales, namespaces);

            return writer.ToString();
        }
    }
}