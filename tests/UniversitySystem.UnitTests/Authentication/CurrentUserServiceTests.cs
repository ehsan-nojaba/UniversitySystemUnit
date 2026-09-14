using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Infrastructure.Services;

namespace UniversitySystem.UnitTests.Authentication;

public class CurrentUserServiceTests
{
    [Fact]
    public void CurrentUserService_WhenUnauthenticated_ReturnsNullAndEmpty()
    {
        // Arrange
        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        var service = new CurrentUserService(httpContextAccessor);

        // Assert
        Assert.Null(service.UserId);
        Assert.False(service.IsAuthenticated);
        Assert.Empty(service.Roles);
        Assert.False(service.IsInRole(RoleNames.Student));
    }

    [Fact]
    public void CurrentUserService_WhenAuthenticated_ReturnsUserIdAndRoles()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "42"),
            new(ClaimTypes.Role, RoleNames.Student),
            new(ClaimTypes.Role, RoleNames.Professor)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        context.User = new ClaimsPrincipal(identity);

        var httpContextAccessor = new HttpContextAccessor { HttpContext = context };
        var service = new CurrentUserService(httpContextAccessor);

        // Assert
        Assert.Equal("42", service.UserId);
        Assert.True(service.IsAuthenticated);
        Assert.Contains(RoleNames.Student, service.Roles);
        Assert.Contains(RoleNames.Professor, service.Roles);
        Assert.True(service.IsInRole(RoleNames.Student));
        Assert.True(service.IsInRole(RoleNames.Professor));
        Assert.False(service.IsInRole(RoleNames.EducationAdmin));
    }
}
