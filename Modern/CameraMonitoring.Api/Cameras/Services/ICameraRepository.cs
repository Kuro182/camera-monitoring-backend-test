namespace CameraMonitoring.Api.Cameras.Services;

public interface ICameraRepository
{
    Task<IReadOnlyList<Camera>> GetActiveAsync(CancellationToken cancellationToken);
}
