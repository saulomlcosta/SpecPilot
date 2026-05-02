using Microsoft.AspNetCore.Mvc;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AiProviderException)
        {
            await WriteAiFailureProblemAsync(context);
        }
        catch (Exception)
        {
            await WriteFailureProblemAsync(context);
        }
    }

    private static async Task WriteAiFailureProblemAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status502BadGateway;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = "Falha no provider de IA.",
            Detail = "Nao foi possivel concluir a operacao com o provider de IA configurado.",
            Status = StatusCodes.Status502BadGateway
        };

        await context.Response.WriteAsJsonAsync(problem);
    }

    private static async Task WriteFailureProblemAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = "Falha interna.",
            Detail = "Ocorreu uma falha inesperada durante o processamento da requisicao.",
            Status = StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
