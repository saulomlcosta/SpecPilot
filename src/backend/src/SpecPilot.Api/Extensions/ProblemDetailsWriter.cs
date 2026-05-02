using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SpecPilot.Api.Extensions;

public static class ProblemDetailsWriter
{
    public static Task WriteAsync(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
