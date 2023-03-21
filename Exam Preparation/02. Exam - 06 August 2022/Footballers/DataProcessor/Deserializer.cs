namespace Footballers.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
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

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCoachDto[]), new XmlRootAttribute("Coaches"));

            var reader = new StringReader(xmlString);

            var coachDtos = (ImportCoachDto[])serializer.Deserialize(reader)!;

            var coaches = new List<Coach>();
            var sb = new StringBuilder();

            foreach (var cDto in coachDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality
                };

                var footballers = new HashSet<Footballer>();

                foreach (var fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = DateTime.ParseExact(fDto.ContractStartDate,"dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType,
                        Coach = coach
                    };

                    if (footballer.ContractEndDate < footballer.ContractStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    footballers.Add(footballer);
                }

                coach.Footballers = footballers;

                coaches.Add(coach);

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            var sb = new StringBuilder();
            var teams = new HashSet<Team>();
            var validFootballerIds  = context.Footballers.Select(f => f.Id).ToArray();

            foreach (var tDto in teamDtos)
            {
                if (!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var team = new Team()
                {
                    Name = tDto.Name,
                    Nationality = tDto.Nationality,
                    Trophies = tDto.Trophies
                };

                var teamsFootballers = new HashSet<TeamFootballer>();

                foreach (var fId in tDto.FootballersIds)
                {
                    if (!validFootballerIds.Contains(fId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    teamsFootballers.Add(new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = fId
                    });
                }
                
                team.TeamsFootballers = teamsFootballers;
                teams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, teamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
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
