using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Constants;

namespace UniversitySystem.Api.Controllers;

/// <summary>
/// مسیرهای موقت بررسی دسترسی نقش‌ها؛ فرایند آموزشی واقعی انجام نمی‌دهد.
/// </summary>
[ApiController]
[Route("api/v1/test")]
public sealed class TestAuthController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public TestAuthController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Student-only test endpoint.
    /// </summary>
    [HttpGet("student")]
    [Authorize(Roles = RoleNames.Student)]
    public IActionResult StudentEndpoint()
    {
        return Ok(new
        {
            Message = "Access granted to Student endpoint.",
            _currentUserService.UserId,
            _currentUserService.Roles
        });
    }

    /// <summary>
    /// Professor-only test endpoint.
    /// </summary>
    [HttpGet("professor")]
    [Authorize(Roles = RoleNames.Professor)]
    public IActionResult ProfessorEndpoint()
    {
        return Ok(new
        {
            Message = "Access granted to Professor endpoint.",
            _currentUserService.UserId,
            _currentUserService.Roles
        });
    }

    /// <summary>
    /// EducationAdmin-only test endpoint.
    /// </summary>
    [HttpGet("admin")]
    [Authorize(Roles = RoleNames.EducationAdmin)]
    public IActionResult AdminEndpoint()
    {
        return Ok(new
        {
            Message = "Access granted to EducationAdmin endpoint.",
            _currentUserService.UserId,
            _currentUserService.Roles
        });
    }
}
