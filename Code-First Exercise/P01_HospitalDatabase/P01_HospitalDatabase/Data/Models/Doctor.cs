namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Doctor
    {
        public Doctor()
        {
            Visitations = new List<Visitation>();
        }

        [Key]
        public int DoctorId { get; set; }

        [MaxLength(ValidationConstants.DoctorNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.DoctorSpecialityMaxLength)]
        public string Speciality { get; set; } = null!;

        public ICollection<Visitation> Visitations { get; set; }
    }
}
