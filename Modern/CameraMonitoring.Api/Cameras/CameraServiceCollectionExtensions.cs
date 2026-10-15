using CameraMonitoring.Api.Cameras.Actions.GetNearest;
using CameraMonitoring.Api.Cameras.Infrastructure;
using CameraMonitoring.Api.Cameras.Services;
using Microsoft.EntityFrameworkCore;

namespace CameraMonitoring.Api.Cameras;

public static class CameraServiceCollectionExtensions
{
    public static IServiceCollection AddCameras(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<CameraDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<GetNearestCamerasHandler>();
        services.AddScoped<ICameraRepository, PostgresCameraRepository>();
        services.AddSingleton<IDistanceCalculator, HaversineDistanceCalculator>();

        return services;
    }
}
