using Microsoft.EntityFrameworkCore;
using MusicHub.Data.Common;
using MusicHub.Data.Models;

namespace MusicHub.Data;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    { }

    public MusicHubDbContext(DbContextOptions options)
        : base(options)
    { }

    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Performer> Performers { get; set; } = null!;
    public DbSet<Producer> Producers { get; set; } = null!;
    public DbSet<Song> Songs { get; set; } = null!;
    public DbSet<SongPerformer> SongsPerformers { get; set; } = null!;
    public DbSet<Writer> Writers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DBConfig.ConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SongPerformer>(entity =>
        {
            entity.HasKey(sp => new { sp.SongId, sp.PerformerId });
        });

        base.OnModelCreating(modelBuilder);
    }
}