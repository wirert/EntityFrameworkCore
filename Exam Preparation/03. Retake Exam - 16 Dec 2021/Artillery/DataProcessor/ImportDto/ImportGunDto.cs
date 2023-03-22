namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportGunDto
    {
        public int ManufacturerId { get; set; }

        [Range(100, 1350000)]
        public int GunWeight { get; set; }

        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Range(1, 100000)]
        public int Range { get; set; }

        [RegularExpression("Howitzer|Mortar|FieldGun|AntiAircraftGun|MountainGun|AntiTankGun")]
        public string GunType { get; set; } = null!;

        public int ShellId { get; set; }

        public HashSet<GunCountryDto> Countries { get; set; }
    }
}
