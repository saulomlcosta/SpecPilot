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
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(error.Type),
            Detail = error.Description,
            Status = statusCode,
            Extensions = { ["code"] = error.Code }
        };

        return controller.StatusCode(statusCode, problemDetails);
    }

    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Requisicao invalida.",
        ErrorType.Unauthorized => "Nao autorizado.",
        ErrorType.Forbidden => "Acesso negado.",
        ErrorType.NotFound => "Recurso nao encontrado.",
        ErrorType.Conflict => "Conflito de negocio.",
        _ => "Falha interna."
    };
}
