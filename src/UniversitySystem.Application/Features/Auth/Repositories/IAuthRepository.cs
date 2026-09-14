using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.Auth.Repositories;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<string[]> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default);
}
