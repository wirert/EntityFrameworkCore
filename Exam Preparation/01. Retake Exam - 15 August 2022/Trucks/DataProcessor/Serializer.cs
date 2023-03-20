namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {            
            var despatchers = context.Despatchers
                .AsNoTracking()
                .Where(d => d.Trucks.Count > 0)
                .OrderByDescending(d => d.Trucks.Count)
                .ThenBy(d => d.Name)
                .Select(d => new DespatcherDto
                {
                    Name = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks.Select(t => new TruckDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        MakeType = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(DespatcherDto[]), new XmlRootAttribute("Despatchers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var writer = new StringWriter();

            serializer.Serialize(writer, despatchers, namespaces);

            return writer.ToString();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .AsNoTracking()
                .Include(c => c.ClientsTrucks)
                .ThenInclude(ct => ct.Truck)
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .OrderByDescending(c => c.ClientsTrucks.Where(t => t.Truck.TankCapacity >= capacity).Count())
                .ThenBy(c => c.Name)
                .Take(10)     
                .AsEnumerable()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                    .Where(t => t.Truck.TankCapacity >= capacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        VinNumber = t.Truck.VinNumber,
                        TankCapacity = t.Truck.TankCapacity,
                        CargoCapacity = t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString()
                    })
                    .OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToArray()
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(clients, Formatting.Indented);

            return result;
        }
    }
}
