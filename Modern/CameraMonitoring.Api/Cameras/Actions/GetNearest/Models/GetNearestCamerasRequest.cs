using System.ComponentModel.DataAnnotations;

namespace CameraMonitoring.Api.Cameras.Actions.GetNearest.Models;

public class GetNearestCamerasRequest
{
    [Required]
    [Range(-90d, 90d)]
    public required double Latitude { get; init; }

    [Required]
    [Range(-180d, 180d)]
    public required double Longitude { get; init; }

    [Required]
    [Range(1, 100)]
    public required int Limit { get; init; }
}
