namespace SoftJail.DataProcessor
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.ImportDto;

    public class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentDtos = JsonConvert.DeserializeObject<List<DepartmentImportDto>>(jsonString);
            var sb = new StringBuilder();
            var departments = new List<Department>();

            foreach (var dDto in departmentDtos)
            {
                if (!IsValid(dDto)
                    || dDto.Cells.Length == 0
                    || dDto.Cells.Any(c => !IsValid(c))
                    )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                departments.Add(new Department()
                {
                    Name = dDto.Name,
                    Cells = dDto.Cells
                            .Select(c => new Cell()
                            {
                                CellNumber = c.CellNumber,
                                HasWindow = c.HasWindow
                            })
                            .ToHashSet()
                });

                sb.AppendLine($"Imported {dDto.Name} with {dDto.Cells.Length} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonerDtos = JsonConvert.DeserializeObject<List<PrisonerImportDto>>(jsonString);
            var sb = new StringBuilder();
            var prisoners = new List<Prisoner>();

            foreach (var pDto in prisonerDtos)
            {
                if (!IsValid(pDto)
                    || pDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? releaseDate = null;
                bool isValidDate = DateTime.TryParseExact
                    (pDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validReleaseDate);

                if (isValidDate)
                {
                    releaseDate = validReleaseDate;
                }

                prisoners.Add(new Prisoner()
                {
                    FullName = pDto.FullName,
                    Nickname = pDto.Nickname,
                    Age = pDto.Age,
                    IncarcerationDate = DateTime.ParseExact(pDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = pDto.Bail,
                    CellId = pDto.CellId,
                    Mails = pDto.Mails.Select(m => new Mail()
                    {
                        Description = m.Description,
                        Sender = m.Sender,
                        Address = m.Address
                    })
                    .ToHashSet()
                });

                sb.AppendLine($"Imported {pDto.FullName} {pDto.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerImportDto[]), new XmlRootAttribute("Officers"));
            var sb = new StringBuilder();
            OfficerImportDto[] officerDtos;
            using var reader = new StringReader(xmlString);
            officerDtos = (OfficerImportDto[])serializer.Deserialize(reader);

            var officers = new List<Officer>();

            foreach (var oDto in officerDtos)
            {
                if (!IsValid(oDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer();

                officer.Salary = oDto.Salary;
                officer.FullName = oDto.FullName;
                officer.Weapon = Enum.Parse<Weapon>(oDto.Weapon);
                officer.Position = Enum.Parse<Position>(oDto.Position);
                officer.DepartmentId = oDto.DepartmentId;
                officer.OfficerPrisoners = oDto.Prisoners
                            .Select(p => new OfficerPrisoner()
                            {
                                PrisonerId = p.Id,
                                Officer = officer
                            })
                            .ToHashSet();

                officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}