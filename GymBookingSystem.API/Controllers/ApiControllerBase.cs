using GymBookingSystem.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace GymBookingSystem.API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    // Translates a failed service result into a ProblemDetails response
    protected ActionResult ToErrorResult(ServiceResult result)
    {
        var statusCode = result.Status switch
        {
            ResultStatus.NotFound => StatusCodes.Status404NotFound,
            ResultStatus.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        return Problem(detail: result.Error, statusCode: statusCode);
    }
}
