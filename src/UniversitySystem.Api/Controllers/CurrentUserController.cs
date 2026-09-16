using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Queries.GetCurrentUser;

namespace UniversitySystem.Api.Controllers;
/// <summary>اطلاعات حساب احراز‌شده برای نمایش نام، نقش و پروفایل در UI.</summary>
[ApiController]
[Route("api/v1/auth/me")]
[Authorize]
public sealed class CurrentUserController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CurrentUserDto>> Get(CancellationToken cancellationToken) => Ok(await sender.Send(new GetCurrentUserQuery(), cancellationToken));
}
