using Microsoft.AspNetCore.Mvc;

namespace SpecPilot.IntegrationTests.Infrastructure;

[ApiController]
[Route("_test/errors")]
public class TestErrorsController : ControllerBase
{
    [HttpGet("unhandled")]
    public IActionResult ThrowUnhandled()
    {
        throw new InvalidOperationException("Erro inesperado de teste");
    }
}
