namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new List<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Position { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; }
    }
}
