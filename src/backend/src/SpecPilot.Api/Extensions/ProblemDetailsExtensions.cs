using Microsoft.AspNetCore.Mvc;
using SpecPilot.Domain.Common;

namespace SpecPilot.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails CreateProblemDetails(this HttpContext context, Error error)
    {
        var statusCode = error.Type.ToStatusCode();

        var problemDetails = new ProblemDetails
        {
            Title = error.Type.ToTitle(),
            Detail = error.Description,
            Status = statusCode
        };

        problemDetails.Extensions["code"] = error.Code;
        return problemDetails;
    }

    public static ProblemDetails CreateProblemDetails(
        this HttpContext context,
        int statusCode,
        string title,
        string detail,
        string code)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode
        };

        problemDetails.Extensions["code"] = code;
        return problemDetails;
    }

    public static int ToStatusCode(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    public static string ToTitle(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Requisicao invalida.",
        ErrorType.Unauthorized => "Nao autorizado.",
        ErrorType.Forbidden => "Acesso negado.",
        ErrorType.NotFound => "Recurso nao encontrado.",
        ErrorType.Conflict => "Conflito de negocio.",
        _ => "Falha interna."
    };
}
