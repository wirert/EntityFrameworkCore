namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Developer
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
    }
}
//•	Id – integer, Primary Key
//•	Name – text (required)
//•	Games - collection of type Game
