using CameraMonitoring.Api.Cameras.Services;
using Xunit;

namespace CameraMonitoring.Api.Tests;

public class HaversineDistanceCalculatorTests
{
    private const double MoscowLatitude = 55.7558;
    private const double MoscowLongitude = 37.6173;
    private const double SaintPetersburgLatitude = 59.9343;
    private const double SaintPetersburgLongitude = 30.3351;
    private const double CalculationTolerance = 0.0000001;

    private readonly HaversineDistanceCalculator _calculator;

    public HaversineDistanceCalculatorTests()
    {
        _calculator = new HaversineDistanceCalculator();
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenPointsAreSame()
    {
        // Arrange
        var latitude = MoscowLatitude;
        var longitude = MoscowLongitude;

        // Act
        var distance = _calculator.Calculate(latitude, longitude, latitude, longitude);

        // Assert
        Assert.InRange(distance, 0, CalculationTolerance);
    }

    [Fact]
    public void Calculate_ShouldReturnSameDistance_WhenPointsAreReversed()
    {
        // Arrange
        var firstLatitude = MoscowLatitude;
        var firstLongitude = MoscowLongitude;
        var secondLatitude = SaintPetersburgLatitude;
        var secondLongitude = SaintPetersburgLongitude;

        // Act
        var distanceFromFirstPoint = _calculator.Calculate(
            firstLatitude,
            firstLongitude,
            secondLatitude,
            secondLongitude);
        var distanceFromSecondPoint = _calculator.Calculate(
            secondLatitude,
            secondLongitude,
            firstLatitude,
            firstLongitude);

        // Assert
        Assert.InRange(
            Math.Abs(distanceFromFirstPoint - distanceFromSecondPoint),
            0,
            CalculationTolerance);
    }

    [Fact]
    public void Calculate_ShouldReturnExpectedDistance_WhenPointsAreKnown()
    {
        // Arrange
        var minimumExpectedDistance = 632d;
        var maximumExpectedDistance = 635d;

        // Act
        var distance = _calculator.Calculate(
            MoscowLatitude,
            MoscowLongitude,
            SaintPetersburgLatitude,
            SaintPetersburgLongitude);

        // Assert
        Assert.InRange(distance, minimumExpectedDistance, maximumExpectedDistance);
    }
}
