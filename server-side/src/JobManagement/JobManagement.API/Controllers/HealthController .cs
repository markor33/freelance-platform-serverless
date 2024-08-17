using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Healthy");
    }
}
