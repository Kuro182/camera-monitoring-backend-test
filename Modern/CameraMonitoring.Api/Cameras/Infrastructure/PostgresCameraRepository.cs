using CameraMonitoring.Api.Cameras.Services;
using Microsoft.EntityFrameworkCore;

namespace CameraMonitoring.Api.Cameras.Infrastructure;

public class PostgresCameraRepository : ICameraRepository
{
    private readonly CameraDbContext _dbContext;

    public PostgresCameraRepository(CameraDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Camera>> GetActiveAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Cameras
            .AsNoTracking()
            .Where(camera => camera.IsActive)
            .ToListAsync(cancellationToken);
    }
}
