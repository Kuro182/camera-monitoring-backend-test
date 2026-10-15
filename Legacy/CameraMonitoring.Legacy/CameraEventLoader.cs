using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using CameraMonitoring.Legacy.Contracts;

namespace CameraMonitoring.Legacy;

public class CameraEventLoader
{
    public List<CameraEvent> LoadEvents(ICameraService service, DateTime from, DateTime to)
    {
        var result = new List<CameraEvent>();

        for (var time = from; time < to; time = time.AddMinutes(10))
        {
            var events = service.GetEvents(time, time.AddMinutes(10));

            foreach (var item in events)
            {
                if (item.Confidence > 0.7)
                    result.Add(item);
            }
        }

        return result;
    }
}

public class CameraEventLoaderRefactored
{
    private const double MinimumConfidence = 0.7;
    private static readonly TimeSpan MaxRequestDuration = TimeSpan.FromMinutes(10);

    private readonly ICameraService _cameraService;

    public CameraEventLoaderRefactored(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }

    public List<CameraEvent> LoadEvents(DateTime from, DateTime to)
    {
        if (from > to)
            throw new ArgumentException("The start of the interval must not be later than its end.", nameof(from));

        if (from.Kind != to.Kind)
            throw new ArgumentException("Both interval boundaries must have the same DateTimeKind.", nameof(to));

        var result = new List<CameraEvent>();
        var seenEvents = new HashSet<(int CameraId, DateTime Timestamp)>();
        var currentFrom = from;

        while (currentFrom < to)
        {
            var currentTo = currentFrom.Add(MaxRequestDuration);

            if (currentTo > to)
                currentTo = to;

            var events = LoadInterval(currentFrom, currentTo);

            foreach (var item in events)
            {
                if (item == null || item.Confidence <= MinimumConfidence)
                    continue;

                var eventKey = (item.CameraId, item.Timestamp);

                if (seenEvents.Add(eventKey))
                    result.Add(item);
            }

            currentFrom = currentTo;
        }

        return result
            .OrderBy(item => item.Timestamp)
            .ThenBy(item => item.CameraId)
            .ToList();
    }

    private CameraEvent[] LoadInterval(DateTime from, DateTime to)
    {
        try
        {
            return _cameraService.GetEvents(from, to) ?? Array.Empty<CameraEvent>();
        }
        catch (TimeoutException exception)
        {
            throw new CameraEventLoadingException(from, to, exception);
        }
        catch (CommunicationException exception)
        {
            throw new CameraEventLoadingException(from, to, exception);
        }
    }
}
