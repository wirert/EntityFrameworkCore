namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportDespatcherDto[]), new XmlRootAttribute("Despatchers"));

            using var reader = new StringReader(xmlString);

            var despatcherDtos = (ImportDespatcherDto[])serializer.Deserialize(reader);

            var despatchers = new List<Despatcher>();
            var sb = new StringBuilder();

            foreach (var dDto in despatcherDtos)
            {
                if (!IsValid(dDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var despatcher = new Despatcher()
                {
                    Name = dDto.Name,
                    Position = dDto.Position,

                };

                var trucks = new List<Truck>();

                foreach (var truck in dDto.Trucks)
                {
                    if (!IsValid(truck))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    trucks.Add(new Truck()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType,
                        Despatcher = despatcher
                    });
                }

                despatcher.Trucks = trucks;

                despatchers.Add(despatcher);

                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }

            context.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();           
        }

        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var clientDtos = JsonConvert.DeserializeObject<List<ImportClientDto>>(jsonString);

            var sb = new StringBuilder();

            var clients = new List<Client>();

            var truckIds = context.Trucks.Select(x => x.Id).ToList();

            foreach (var cDto in clientDtos)
            {
                if (!IsValid(cDto) || cDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var client = new Client()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality,
                    Type = cDto.Type,
                };

                var clientTrucks = new List<ClientTruck>();

                foreach (var truckId in cDto.TruckIds)
                {
                    if (!truckIds.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    clientTrucks.Add(new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    });
                }

                client.ClientsTrucks = clientTrucks;
                clients.Add(client);

                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}