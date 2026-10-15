using Microsoft.EntityFrameworkCore;

namespace CameraMonitoring.Api.Cameras.Infrastructure;

public class CameraDbContext : DbContext
{
    public CameraDbContext(DbContextOptions<CameraDbContext> options)
        : base(options)
    {
    }

    public DbSet<Camera> Cameras => Set<Camera>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var camera = modelBuilder.Entity<Camera>();

        camera.ToTable("Cameras");
        camera.HasKey(item => item.Id);
        camera.Property(item => item.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
