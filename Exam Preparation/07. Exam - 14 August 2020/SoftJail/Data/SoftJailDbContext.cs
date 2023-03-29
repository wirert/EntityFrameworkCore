namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
    {
        public SoftJailDbContext()
        {
        }

        public SoftJailDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Cell> Cells { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Mail> Mails { get; set; }
        public virtual DbSet<Officer> Officers { get; set; }
        public virtual DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
        public virtual DbSet<Prisoner> Prisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OfficerPrisoner>(entity =>
                {
                    entity.HasKey(k => new { k.OfficerId, k.PrisonerId });

                    entity.HasOne(p => p.Prisoner)
                          .WithMany(po => po.PrisonerOfficers)
                          .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(o => o.Officer)
                          .WithMany(op => op.OfficerPrisoners)
                          .OnDelete(DeleteBehavior.Restrict);
                });

        }
    }
}