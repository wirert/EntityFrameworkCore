namespace Artillery.DataProcessor
{
    using System.Xml.Serialization;

    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using Data;
    using Data.Models.Enums;
    using DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .AsNoTracking()
                .Where(s => s.ShellWeight > shellWeight)
                .OrderBy(s => s.ShellWeight)
                .Select(s => new
                {
                    s.ShellWeight,
                    s.Caliber,
                    Guns = s.Guns
                    .Where(g => g.GunType == GunType.AntiAircraftGun)
                    .OrderByDescending(g => g.GunWeight)
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        g.GunWeight,
                        g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .ToArray()
                })
                .ToArray();

           return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(GunDto[]), new XmlRootAttribute("Guns"));

            var guns = context.Guns
                .AsNoTracking()
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .Select(g => new GunDto
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                    .Where(cg => cg.Country.ArmySize > 4500000)
                    .OrderBy(cg => cg.Country.ArmySize)
                    .Select(cg => new CountryDto
                    {
                        CountryName = cg.Country.CountryName,
                        ArmySize = cg.Country.ArmySize
                    })
                    .ToArray()
                })
                .ToArray();

            serializer.Serialize(writer, guns, namespaces);

            return writer.ToString();
        }
    }
}
