using Microsoft.AspNetCore.Diagnostics;
using SpecPilot.Api.Extensions;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is AiProviderException)
        {
            _logger.LogError(
                exception,
                "Falha controlada no provider de IA. TraceId={TraceId} Path={Path}",
                httpContext.TraceIdentifier,
                httpContext.Request.Path);

            var aiProblem = httpContext.CreateProblemDetails(
                StatusCodes.Status502BadGateway,
                "Falha no provider de IA.",
                "Nao foi possivel concluir a operacao com o provider de IA configurado.",
                "ai.provider_failure");

            await ProblemDetailsWriter.WriteAsync(httpContext, aiProblem);
            return true;
        }

        _logger.LogError(
            exception,
            "Falha inesperada. TraceId={TraceId} Method={Method} Path={Path}",
            httpContext.TraceIdentifier,
            httpContext.Request.Method,
            httpContext.Request.Path);

        var problemDetails = httpContext.CreateProblemDetails(
            StatusCodes.Status500InternalServerError,
            "Falha interna.",
            "Ocorreu uma falha inesperada durante o processamento da requisicao.",
            "common.internal_error");

        await ProblemDetailsWriter.WriteAsync(httpContext, problemDetails);
        return true;
    }
}
