using System;

namespace CameraMonitoring.Legacy;

public class CameraEventLoadingException : Exception
{
    public CameraEventLoadingException(DateTime from, DateTime to, Exception innerException)
        : base($"Failed to load camera events for the interval [{from:O}, {to:O}).", innerException)
    {
        From = from;
        To = to;
    }

    public DateTime From { get; }

    public DateTime To { get; }
}
