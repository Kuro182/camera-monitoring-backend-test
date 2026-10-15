namespace CameraMonitoring.Api.Cameras.Services;

public class HaversineDistanceCalculator : IDistanceCalculator
{
    private const double EarthRadiusKm = 6371;

    public double Calculate(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        var latitude1Radians = ToRadians(latitude1);
        var latitude2Radians = ToRadians(latitude2);
        var latitudeDifference = ToRadians(latitude2 - latitude1);
        var longitudeDifference = ToRadians(longitude2 - longitude1);

        var latitudeSin = Math.Sin(latitudeDifference / 2);
        var longitudeSin = Math.Sin(longitudeDifference / 2);

        var latitudePart = latitudeSin * latitudeSin;
        var longitudePart = Math.Cos(latitude1Radians) * Math.Cos(latitude2Radians)
            * longitudeSin * longitudeSin;

        var a = latitudePart + longitudePart;

        a = Math.Clamp(a, 0, 1);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
