namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class Diagnose
    {
        [Key]
        public int DiagnoseId { get; set; }

        [MaxLength(ValidationConstants.DiagnoseNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.DiagnoseCommentsMaxLength)]
        public string? Comments { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; } = null!;
    }
}
