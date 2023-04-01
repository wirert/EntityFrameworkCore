using System.Text;
using System.Xml.Serialization;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Boardgames.Data;
using Boardgames.DataProcessor.ExportDto;

namespace Boardgames.DataProcessor
{
    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Include(c => c.Boardgames)
                .AsNoTracking()
                .Where(c => c.Boardgames.Any())
                .AsEnumerable()
                .Select(c => new CreatorExportDto()
                {
                    BoardgamesCount = c.Boardgames.Count,
                    FullName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                        .Select(b => new BoardgameExportDto()
                        {
                            Name = b.Name,
                            YearPublished = b.YearPublished
                        })
                        .OrderBy(b => b.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.FullName)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var serializer = new XmlSerializer(typeof(CreatorExportDto[]), new XmlRootAttribute("Creators"));
            var sb = new StringBuilder();
            using var writer = new StringWriter(sb);
            serializer.Serialize(writer, creators, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .AsNoTracking()
                .Where(s => s.BoardgamesSellers
                                .Any(bs => bs.Boardgame.YearPublished >= year
                                        && bs.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers
                            .Where(bs => bs.Boardgame.YearPublished >= year
                                        && bs.Boardgame.Rating <= rating)
                            .Select(bs => new
                            {
                                bs.Boardgame.Name,
                                bs.Boardgame.Rating,
                                bs.Boardgame.Mechanics,
                                Category = bs.Boardgame.CategoryType.ToString()
                            })
                            .OrderByDescending(b => b.Rating)
                            .ThenBy(b => b.Name)
                            .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}