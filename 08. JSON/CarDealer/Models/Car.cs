
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        [NotMapped]
        public decimal Price => this.PartsCars.Sum(cp => cp.Part.Price);

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();    

        public ICollection<PartCar> PartsCars { get; set; } = new List<PartCar>();
    }
}
