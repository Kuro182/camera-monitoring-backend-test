namespace CameraMonitoring.Api.Cameras.Services;

public interface IDistanceCalculator
{
    double Calculate(double latitude1, double longitude1, double latitude2, double longitude2);
}
