using UniversitySystem.Application.Features.Auth.Commands.Login;

namespace UniversitySystem.Application.Features.Auth.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
