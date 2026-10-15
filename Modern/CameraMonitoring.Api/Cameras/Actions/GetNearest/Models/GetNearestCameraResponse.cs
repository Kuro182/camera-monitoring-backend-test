namespace CameraMonitoring.Api.Cameras.Actions.GetNearest.Models;

public class GetNearestCameraResponse
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public double DistanceKm { get; set; }
}
