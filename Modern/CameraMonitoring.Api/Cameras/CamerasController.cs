using CameraMonitoring.Api.Cameras.Actions.GetNearest;
using CameraMonitoring.Api.Cameras.Actions.GetNearest.Models;
using Microsoft.AspNetCore.Mvc;

namespace CameraMonitoring.Api.Cameras;

[ApiController]
[Route("api/cameras")]
public class CamerasController : ControllerBase
{
    private readonly GetNearestCamerasHandler _handler;

    public CamerasController(GetNearestCamerasHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("nearest")]
    [ProducesResponseType<IReadOnlyList<GetNearestCameraResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<GetNearestCameraResponse>>> GetNearest(
        [FromBody] GetNearestCamerasRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _handler.HandleAsync(request, cancellationToken);

        return Ok(response);
    }
}
