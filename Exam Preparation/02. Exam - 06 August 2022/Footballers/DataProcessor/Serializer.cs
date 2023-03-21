namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .AsNoTracking()
                .Where(c => c.Footballers.Count > 0)
                .OrderByDescending(c => c.Footballers.Count)
                .ThenBy(c => c.Name)
                .Select(c => new CoachDto()
                {
                    Name = c.Name,
                    FootballersCount = c.Footballers.Count,
                    Footballers = c.Footballers
                        .OrderBy(f => f.Name)
                        .Select(f => new FootBallerDto()
                        {
                            Name = f.Name,
                            PositionType = f.PositionType.ToString()
                        })
                        .ToArray()
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var serializer = new XmlSerializer(typeof(CoachDto[]), new XmlRootAttribute("Coaches"));
            var writer = new StringWriter();

            serializer.Serialize(writer, coaches, namespaces);

            return writer.ToString();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .AsNoTracking()
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .OrderByDescending(t => t.TeamsFootballers.Count(f => f.Footballer.ContractStartDate >= date))
                .ThenBy(t => t.Name)
                .Take(5)
                .Include(t => t.TeamsFootballers)
                .ThenInclude(tf => tf.Footballer)
                //.AsEnumerable()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                            .Where(tf => tf.Footballer.ContractStartDate >= date)
                            .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                            .ThenBy(tf => tf.Footballer.Name)
                            .Select(f => new
                            {
                                FootballerName = f.Footballer.Name,
                                ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                                ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                                BestSkillType = f.Footballer.BestSkillType.ToString(),
                                PositionType = f.Footballer.PositionType.ToString()
                            })
                            .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
