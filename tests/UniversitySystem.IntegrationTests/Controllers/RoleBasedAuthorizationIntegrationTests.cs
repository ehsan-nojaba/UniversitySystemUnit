using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Common;
using UniversitySystem.Domain.Constants;
using UniversitySystem.Domain.Entities;
using Xunit;
using UniversitySystem.Application.Common.Logic;

namespace UniversitySystem.IntegrationTests.Controllers;

public class RoleBasedAuthorizationIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly UniversityApiFactory _factory;
    private readonly HttpClient _client;
    private readonly ITokenService _tokenService;

    public RoleBasedAuthorizationIntegrationTests(UniversityApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    }

    private string CreateTokenForUser(long userId, string username, params string[] roles)
    {
        var user = UserLogic.Create(username,"DummyPasswordHash","Test",username);
        // Set Id using reflection since Id has a protected setter on BaseEntity
        typeof(BaseEntity)
            .GetProperty(nameof(BaseEntity.Id))?
            .SetValue(user, userId);

        var (token, _) = _tokenService.GenerateToken(user, roles);
        return token;
    }

    [Fact]
    public async Task GetStudentEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/test/student");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetStudentEndpoint_WithWrongRoleProfessor_ReturnsForbidden()
    {
        // Arrange
        var token = CreateTokenForUser(101, "prof_user", RoleNames.Professor);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/test/student");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetStudentEndpoint_WithStudentToken_ReturnsOk()
    {
        // Arrange
        var token = CreateTokenForUser(102, "student_user", RoleNames.Student);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/test/student");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProfessorEndpoint_WithProfessorToken_ReturnsOk()
    {
        // Arrange
        var token = CreateTokenForUser(103, "prof_user2", RoleNames.Professor);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/test/professor");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminEndpoint_WithEducationAdminToken_ReturnsOk()
    {
        // Arrange
        var token = CreateTokenForUser(104, "admin_user", RoleNames.EducationAdmin);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/test/admin");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminEndpoint_WithStudentToken_ReturnsForbidden()
    {
        // Arrange
        var token = CreateTokenForUser(105, "student_user2", RoleNames.Student);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/test/admin");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}

