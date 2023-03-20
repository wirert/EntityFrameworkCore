namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Data.Models.Enums;

    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new List<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(8)]
        [Required]
        public string RegistrationNumber { get; set; }

        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;

        public int TankCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        public int DespatcherId { get; set; }

        [ForeignKey(nameof(DespatcherId))]
        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
