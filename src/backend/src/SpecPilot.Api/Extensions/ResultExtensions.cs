using Microsoft.AspNetCore.Mvc;
using SpecPilot.Domain.Common;

namespace SpecPilot.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value!)
            : controller.ToProblemDetails(result.Error!);
    }

    public static IActionResult ToActionResult(
        this ControllerBase controller,
        Result result,
        Func<IActionResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess()
            : controller.ToProblemDetails(result.Error!);
    }

    public static IActionResult ToProblemDetails(this ControllerBase controller, Error error)
    {
        var problemDetails = controller.HttpContext.CreateProblemDetails(error);
        return controller.StatusCode(problemDetails.Status ?? StatusCodes.Status500InternalServerError, problemDetails);
    }
}
