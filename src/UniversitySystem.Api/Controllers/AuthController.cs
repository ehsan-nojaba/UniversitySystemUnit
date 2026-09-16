using UniversitySystem.Api.Contracts;
using Mapster;
using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.Auth.Commands.Login;

namespace UniversitySystem.Api.Controllers;
/// <summary>
/// ورودی HTTP بخش «ورود و دریافت توکن»؛ نقش مجاز را تعیین می‌کند و عملیات را به MediatR می‌سپارد.
/// </summary>
public sealed class AuthController : ApiControllerBase
{
    private readonly ISender _mediator;
    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(Summary = "ورود به سامانه", Description = "نام کاربری و رمز عبور را بررسی می‌کند و توکن ورود را برمی‌گرداند. این مسیر نیاز به توکن ندارد.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(LoginResponse))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var param = request.Adapt<LoginCommand>();
        var response = await _mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}
