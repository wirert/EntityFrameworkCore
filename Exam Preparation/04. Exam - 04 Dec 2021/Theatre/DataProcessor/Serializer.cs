namespace Theatre.DataProcessor
{
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using Data;
    using DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                    .AsNoTracking()
                    .Include(t => t.Tickets)
                    .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                    .OrderByDescending(t => t.NumberOfHalls)
                    .ThenBy(t => t.Name)
                    .Select(t => new
                    {
                        t.Name,
                        Halls = t.NumberOfHalls,
                        TotalIncome = t.Tickets.Where(p => p.RowNumber <= 5).Sum(p => p.Price),
                        Tickets = t.Tickets
                            .Where(p => p.RowNumber <= 5)
                            .OrderByDescending(p => p.Price)
                            .Select(p => new
                            {
                                p.Price,
                                p.RowNumber
                            })
                            .ToArray()
                    })
                    .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var playDtos = context.Plays
                .AsNoTracking()
                .Include(p => p.Casts)
                .Where(p => p.Rating <= raiting)
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .Select(p => new PlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                        .Where(a => a.IsMainCharacter)
                        .OrderByDescending(a => a.FullName)
                        .Select(a => new ActorDto()
                        {
                            FullName = a.FullName,
                            MainCharacter = $"Plays main character in '{p.Title}'."
                        })
                        .ToArray()
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var serializer = new XmlSerializer(typeof(PlayDto[]), new XmlRootAttribute("Plays"));
            using var writer = new StringWriter();

            serializer.Serialize(writer, playDtos, namespaces);

            return writer.ToString();
        }
    }
}
