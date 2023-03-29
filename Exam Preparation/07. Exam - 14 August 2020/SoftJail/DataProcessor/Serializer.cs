namespace SoftJail.DataProcessor
{

    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using Data;
    using DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners                               
                                .AsNoTracking()                                
                                .Where(p => ids.Contains(p.Id))
                                .Select(p => new
                                {
                                    p.Id,
                                    Name = p.FullName,
                                    CellNumber = p.Cell.CellNumber,
                                    Officers = p.PrisonerOfficers
                                        .Select(po => new
                                        {
                                            OfficerName = po.Officer.FullName,
                                            Department = po.Officer.Department.Name,
                                        })
                                        .OrderBy(o => o.OfficerName)
                                        .ToArray(),
                                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                                })
                                .OrderBy(p => p.Name)
                                .ThenBy(p => p.Id)
                                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonersNamesToExtract = prisonersNames.Split(',');

            var prisoners = context.Prisoners
                .AsNoTracking()
                .Where(p => prisonersNamesToExtract.Contains(p.FullName))
                .Select(p => new PrisonerExportDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                                        .Select(m => new MailExportDto()
                                        {
                                            Description = string.Join("", m.Description.Reverse()),
                                        })
                                        .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var serializer = new XmlSerializer(typeof(PrisonerExportDto[]), new XmlRootAttribute("Prisoners"));
            var sb = new StringBuilder();

            using var writer = new StringWriter(sb);
            serializer.Serialize(writer, prisoners, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
