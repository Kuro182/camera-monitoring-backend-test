using Microsoft.EntityFrameworkCore;

namespace CameraMonitoring.Api.Cameras.Infrastructure;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CameraDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Cameras.AnyAsync())
            return;

        dbContext.Cameras.AddRange(
            new Camera
            {
                Id = 1,
                Name = "Forest-Tower-01",
                Latitude = 55.7520,
                Longitude = 37.6175,
                IsActive = true
            },
            new Camera
            {
                Id = 2,
                Name = "Pine-Ridge-02",
                Latitude = 55.7033,
                Longitude = 37.5300,
                IsActive = true
            },
            new Camera
            {
                Id = 3,
                Name = "North-Sector-03",
                Latitude = 55.8298,
                Longitude = 37.6330,
                IsActive = true
            },
            new Camera
            {
                Id = 4,
                Name = "Reserve-Post-04",
                Latitude = 55.7558,
                Longitude = 37.6173,
                IsActive = false
            });

        await dbContext.SaveChangesAsync();
    }
}
