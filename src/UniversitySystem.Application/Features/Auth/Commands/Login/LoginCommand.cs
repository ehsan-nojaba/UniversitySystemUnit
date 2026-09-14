using MediatR;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command to authenticate an existing active user and issue a JWT token.
/// </summary>
/// <param name="Username">User's unique username / account identifier.</param>
/// <param name="Password">User's plain-text password.</param>
public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
