using Microsoft.AspNetCore.Mvc;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// کلاس پایه مشترک کنترلرهای API؛ محل ویژگی‌های مشترک HTTP است.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
