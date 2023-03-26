namespace VaporStore.DataProcessor
{
    using System.Globalization;
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using Data;
    using DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .AsEnumerable()
                .Select(g => new
                {
                    g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(m => m.Purchases.Any())
                        .Select(m => new
                        {
                            m.Id,
                            Title = m.Name,
                            Developer = m.Developer.Name,
                            Tags = string.Join(", ", m.GameTags.Select(gt => gt.Tag.Name)),
                            Players = m.Purchases.Count
                        })
                        .OrderByDescending(g => g.Players)
                        .ThenBy(g => g.Id)
                        .ToArray(),
                    TotalPlayers = g.Games.Sum(m => m.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            return JsonConvert.SerializeObject(genres, Formatting.Indented);
        }    

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            var users = context.Users
                .AsEnumerable()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Purchises = u.Cards
                        .SelectMany(c => c.Purchases
                                    .Where(p => p.Type.ToString() == purchaseType)
                                    .Select(p => new PurchiseDto
                                    {
                                        CardNumber = p.Card.Number,
                                        Cvc = p.Card.Cvc,
                                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                                        Game = new GameDto
                                        {
                                            Title = p.Game.Name,
                                            Genre = p.Game.Genre.Name.ToString(),
                                            Price = p.Game.Price
                                        }
                                    }))
                                    .OrderBy(p => p.Date)
                                    .ToArray(),
                    TotalSpent = u.Cards.Sum(c => c.Purchases
                                                .Where(p => p.Type.ToString() == purchaseType)
                                                .Sum(p => p.Game.Price))

                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));

            using var writer = new StringWriter();

            serializer.Serialize(writer, users, namespaces);

            return writer.ToString();
        }
    }
}