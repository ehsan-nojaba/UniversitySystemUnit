using MediatR;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command to authenticate an existing active user and issue a JWT token.
/// </summary>
public class LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
