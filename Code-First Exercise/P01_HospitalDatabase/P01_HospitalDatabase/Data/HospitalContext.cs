namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;

    using Common;
    using Data.Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext()
        { }

        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options) 
        { }

        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Diagnose> Diagnoses { get; set; } = null!;
        public DbSet<Medicament> Medicaments { get; set; } = null!;
        public DbSet<Visitation> Visitations { get; set; } = null!;
        public DbSet<PatientMedicament> PatientMedicaments { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.Email).IsUnicode(false);
            });

            modelBuilder.Entity<PatientMedicament>(entity => entity.HasKey("PatientId", "MedicamentId"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
