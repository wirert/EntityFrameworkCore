namespace Artillery.DataProcessor
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
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCountryDto[]), new XmlRootAttribute("Countries"));
            var reader = new StringReader(xmlString);

            var countryDtos = (ImportCountryDto[])serializer.Deserialize(reader)!;

            var countries = new HashSet<Country>();
            var sb = new StringBuilder();

            foreach (var cDto in countryDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                countries.Add(new Country()
                {
                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize
                });

                sb.AppendLine(string.Format(SuccessfulImportCountry, cDto.CountryName, cDto.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportManufacturerDto[]), new XmlRootAttribute("Manufacturers"));

            var reader = new StringReader(xmlString);

            var manuDtos = (ImportManufacturerDto[])serializer.Deserialize(reader)!;
            var manufacturers = new HashSet<Manufacturer>();
            var sb = new StringBuilder();

            foreach (var mDto in manuDtos)
            {
                if (!IsValid(mDto) || manufacturers.Any(m => m.ManufacturerName == mDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturers.Add(new Manufacturer()
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded
                });

                string[] founded = mDto.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string country = founded.Last();
                string town = founded[founded.Length - 2];

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, mDto.ManufacturerName, $"{town}, {country}"));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportShellDto[]), new XmlRootAttribute("Shells"));

            var reader = new StringReader(xmlString);

            var shellDtos = (ImportShellDto[])serializer.Deserialize(reader)!;
            var shells = new HashSet<Shell>();
            var sb = new StringBuilder();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                shells.Add(new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                });

                sb.AppendLine(string.Format(SuccessfulImportShell, shellDto.Caliber, shellDto.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunDtos = JsonConvert.DeserializeObject<HashSet<ImportGunDto>>(jsonString);

            var guns = new HashSet<Gun>();
            var sb = new StringBuilder();

            foreach (var gDto in gunDtos)
            {
                if (!IsValid(gDto) || gDto.GunType == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun()
                {
                    BarrelLength = gDto.BarrelLength,
                    GunType = Enum.Parse<GunType>(gDto.GunType),
                    GunWeight = gDto.GunWeight,
                    ManufacturerId = gDto.ManufacturerId,
                    ShellId = gDto.ShellId,
                    NumberBuild = gDto.NumberBuild,
                    Range = gDto.Range,
                    CountriesGuns = gDto.Countries.Select(c => new CountryGun()
                    {
                        CountryId = c.Id
                    })
                    .ToHashSet()
                };

                guns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gDto.GunType.ToString(), gDto.GunWeight, gDto.BarrelLength));
            }

            context.Guns.AddRange(guns);
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