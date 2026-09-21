using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using UniversitySystem.Application.Features.Auth.Commands.Login;
using Xunit;

namespace UniversitySystem.IntegrationTests.Controllers;

public class AuthControllerIntegrationTests : IClassFixture<UniversityApiFactory>
{
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(UniversityApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WhenCredentialsEmpty_ReturnsBadRequestWithBusinessError()
    {
        // Arrange
        var command = new LoginCommand { Username = "", Password = "" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal("Business Rule Violation", problemDetails.Title);
        Assert.Contains("الزامی", problemDetails.Detail!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Login_WhenUserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var command = new LoginCommand { Username = "non_existent_user_xyz", Password = "RandomPassword123!" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", command);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal(401, problemDetails.Status);
        Assert.Equal("Unauthorized", problemDetails.Title);
    }
}
