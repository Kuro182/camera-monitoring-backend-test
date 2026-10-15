using CameraMonitoring.Api.Cameras.Actions.GetNearest.Models;
using CameraMonitoring.Api.Cameras.Services;

namespace CameraMonitoring.Api.Cameras.Actions.GetNearest;

public class GetNearestCamerasHandler
{
    private readonly ICameraRepository _cameraRepository;
    private readonly IDistanceCalculator _distanceCalculator;

    public GetNearestCamerasHandler(
        ICameraRepository cameraRepository,
        IDistanceCalculator distanceCalculator)
    {
        _cameraRepository = cameraRepository;
        _distanceCalculator = distanceCalculator;
    }

    public async Task<IReadOnlyList<GetNearestCameraResponse>> HandleAsync(
        GetNearestCamerasRequest request,
        CancellationToken cancellationToken)
    {
        var cameras = await _cameraRepository.GetActiveAsync(cancellationToken);
        var camerasWithDistance = new List<GetNearestCameraResponse>();

        foreach (var camera in cameras)
        {
            var distance = _distanceCalculator.Calculate(
                request.Latitude,
                request.Longitude,
                camera.Latitude,
                camera.Longitude);

            camerasWithDistance.Add(new GetNearestCameraResponse
            {
                Id = camera.Id,
                Name = camera.Name,
                DistanceKm = distance
            });
        }

        var nearestCameras = camerasWithDistance
            .OrderBy(camera => camera.DistanceKm)
            .ThenBy(camera => camera.Id)
            .Take(request.Limit)
            .ToList();

        foreach (var camera in nearestCameras)
        {
            camera.DistanceKm = Math.Round(camera.DistanceKm, 2);
        }

        return nearestCameras;
    }
}
