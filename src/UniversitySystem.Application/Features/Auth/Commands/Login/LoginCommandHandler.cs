using MediatR;
using UniversitySystem.Application.Features.Auth.Services;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        => authService.LoginAsync(request.Username, request.Password, cancellationToken);
}
