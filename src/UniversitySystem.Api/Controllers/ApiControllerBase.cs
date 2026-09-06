using Microsoft.AspNetCore.Mvc;

namespace UniversitySystem.Api.Controllers;

/// <summary>
/// Base API controller defining global route conventions and API behaviors.
/// Route format: /api/v1/[controller]
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
