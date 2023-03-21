namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;

    public class Footballer
    {
        public Footballer()
        {
            TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public PositionType PositionType { get; set; }

        public BestSkillType BestSkillType { get; set; }

        public int CoachId { get; set; }

        [ForeignKey(nameof(CoachId))]
        public virtual Coach Coach { get; set; } = null!;

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
