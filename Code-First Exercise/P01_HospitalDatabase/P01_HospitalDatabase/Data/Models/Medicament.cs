﻿namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Medicament
    {
        public Medicament()
        {
            Prescriptions = new List<PatientMedicament>();
        }

        [Key]
        public int MedicamentId { get; set; }

        [MaxLength(ValidationConstants.MedicamentNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
