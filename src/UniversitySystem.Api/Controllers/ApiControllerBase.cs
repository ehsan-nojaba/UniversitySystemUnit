using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// کلاس پایه مشترک کنترلرهای API؛ محل ویژگی‌های مشترک HTTP است.
/// </summary>
[ApiController]
public abstract class ApiControllerBase(ISender mediator) : ControllerBase
{
    protected ISender Mediator { get; } = mediator;
}
