using Swashbuckle.AspNetCore.Annotations;
using UniversitySystem.Api.Infrastructure;
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
public sealed class CurrentUserController(ISender _mediator) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "مشاهده اطلاعات", Description = "عملیات مشاهده اطلاعات؛ دسترسی مطابق نقش مجاز این مسیر است.")]
    [SwaggerResponse(200, "عملیات موفق", typeof(CurrentUserDto))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var param = new GetCurrentUserQuery();
        var response = await _mediator.Send(param, cancellationToken);
        return response.ToApiResponse();
    }
}
