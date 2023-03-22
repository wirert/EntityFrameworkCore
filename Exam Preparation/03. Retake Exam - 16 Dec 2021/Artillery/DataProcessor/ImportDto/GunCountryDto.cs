namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class GunCountryDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
