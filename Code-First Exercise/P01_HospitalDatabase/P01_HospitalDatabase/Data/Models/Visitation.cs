namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class Visitation
    {
        [Key]
        public int VisitationId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(ValidationConstants.VisitationCommentsMaxLength)]
        public string? Comments { get; set; }


        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; } = null!;
    }
}
