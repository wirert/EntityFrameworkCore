namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using Common;

    public class Patient
    {
        public Patient()
        {
            Visitations = new List<Visitation>();
            Diagnoses = new List<Diagnose>();
            Prescriptions = new List<PatientMedicament>();
        }

        [Key]
        public int PatientId { get; set; }

        [MaxLength(ValidationConstants.PatientNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [MaxLength(ValidationConstants.PatientNameMaxLength)]
        public string LastName { get; set; } = null!;

        [MaxLength(ValidationConstants.PatientAddressMaxLength)]
        public string Address { get; set; } = null!;

        [EmailAddress]
        [Unicode(false)]
        [MaxLength(ValidationConstants.PatientEmailMaxLength)]
        public string? Email { get; set; }

        public bool HasInsurance { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; }

        public virtual ICollection<Diagnose> Diagnoses { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
