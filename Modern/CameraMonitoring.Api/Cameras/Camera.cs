namespace CameraMonitoring.Api.Cameras;

public class Camera
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool IsActive { get; set; }
}
