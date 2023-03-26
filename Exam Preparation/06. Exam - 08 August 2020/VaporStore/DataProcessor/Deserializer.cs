namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.ImportDto;
    using System.Xml.Serialization;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gameDtos = JsonConvert.DeserializeObject<List<GameImportDto>>(jsonString);
            var games = new List<Game>();
            var devs = new List<Developer>();
            var tags = new List<Tag>();
            var genres = new List<Genre>();
            var sb = new StringBuilder();

            foreach (var gDto in gameDtos!)
            {
                if (!IsValid(gDto)
                    || gDto.Tags!.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var game = new Game();
                game.Name = gDto.Name!;
                game.Price = gDto.Price;
                game.ReleaseDate = DateTime.ParseExact(gDto.ReleaseDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var dev = devs.FirstOrDefault(d => d.Name == gDto.Developer);

                if (dev == null)
                {
                    dev = new Developer() { Name = gDto.Developer! };
                    devs.Add(dev);
                }
                game.Developer = dev;

                var genre = genres.FirstOrDefault(g => g.Name == gDto.Genre);

                if (genre == null)
                {
                    genre = new Genre() { Name = gDto.Genre! };
                    genres.Add(genre);
                }
                game.Genre = genre;

                foreach (var gameTag in gDto.Tags)
                {
                    var tag = tags.FirstOrDefault(t => t.Name == gameTag);

                    if (tag == null)
                    {
                        tag = new Tag() { Name = gameTag };
                        tags.Add(tag);
                    }
                    game.GameTags.Add( new GameTag() { Tag = tag });
                }

                games.Add(game);

                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();            

            return sb.ToString();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var userDtos = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString);
            var users = new List<User>();
            var sb = new StringBuilder();

            foreach (var uDto in userDtos!)
            {
                if (!IsValid(uDto) 
                    || !uDto.Cards!.Any()
                    || uDto.Cards!.Any(c => !IsValid(c))
                    )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var user = new User()
                {
                    FullName = uDto.FullName!,
                    Username = uDto.Username!,
                    Email = uDto.Email!,
                    Age = uDto.Age,
                    Cards = uDto.Cards!.Select(c => new Card()
                    {
                        Number = c.Number!,
                        Cvc = c.Cvc!,
                        Type = Enum.Parse<CardType>(c.Type!)
                    })
                    .ToHashSet()
                };

               users.Add(user);

                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(users);
            context.SaveChanges();
           
            return sb.ToString();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));
            var sb = new StringBuilder();
            var purchases = new List<Purchase>();
            using var reader = new StringReader(xmlString);
            var puchaseDtos = (PurchaseImportDto[])serializer.Deserialize(reader)!;

            foreach (var pDto in puchaseDtos)
            {     
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var card = context.Cards.FirstOrDefault(c => c.Number == pDto.CardNumber);
                var game = context.Games.FirstOrDefault(g => g.Name == pDto.GameName);

                if (card == null || game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var purchise = new Purchase()
                {
                    Game = game,
                    Card = card,
                    Type = Enum.Parse<PurchaseType>(pDto.Type!),
                    ProductKey = pDto.ProductKey!,
                    Date = DateTime.ParseExact(pDto.Date!, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                purchases.Add(purchise);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, purchise.Game.Name, card.User.Username));
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}