namespace Theatre.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportPlayDto[]), new XmlRootAttribute("Plays"));
            var playDtos = (ImportPlayDto[])serializer.Deserialize(new StringReader(xmlString))!;
            var plays = new HashSet<Play>();
            var sb = new StringBuilder();

            foreach (var playDto in playDtos)
            {
                var isValidDuration = TimeSpan.TryParse(playDto.Duration, out var playDuration);

                if (!IsValid(playDto) || !isValidDuration || playDuration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                plays.Add(new Play()
                {
                    Title = playDto.Title,
                    Duration = playDuration,
                    Rating = playDto.Rating,
                    Genre = Enum.Parse<Genre>(playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                });

                sb.AppendLine(string.Format(SuccessfulImportPlay, playDto.Title, playDto.Genre, playDto.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCastDto[]), new XmlRootAttribute("Casts"));
            var castDtos = (ImportCastDto[])serializer.Deserialize(new StringReader(xmlString))!;
            var casts = new HashSet<Cast>();
            var sb = new StringBuilder();

            foreach (var cast in castDtos)
            {
                if (!IsValid(cast))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                casts.Add(new Cast()
                {
                    FullName = cast.FullName,
                    IsMainCharacter = cast.IsMainCharacter,
                    PhoneNumber = cast.PhoneNumber,
                    PlayId = cast.PlayId
                });

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatreDtos = JsonConvert.DeserializeObject<List<ImportTheatreDto>>(jsonString);
            var theatres = new List<Theatre>();
            var sb = new StringBuilder();

            foreach (var tDto in theatreDtos!)
            {
                if (!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre()
                {
                    Name = tDto.Name,
                    NumberOfHalls = tDto.NumberOfHalls,
                    Director = tDto.Director
                };

                var tickets = new HashSet<Ticket>();

                foreach (var t in tDto.Tickets)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    tickets.Add(new Ticket()
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                        PlayId = t.PlayId,
                        Theatre = theatre
                    });
                }

                theatre.Tickets = tickets;
                theatres.Add(theatre);

                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
