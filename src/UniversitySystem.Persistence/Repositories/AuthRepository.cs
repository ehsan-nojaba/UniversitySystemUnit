using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.Auth.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class AuthRepository(ApplicationDbContext context) : IAuthRepository
{
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await (
            from u in context.Users.AsNoTracking()
            where u.Username == username
            select u
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string[]> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default)
    {
        var roles = await (
            from ur in context.UserRoles.AsNoTracking()
            join r in context.Roles.AsNoTracking() on ur.RoleId equals r.Id
            where ur.UserId == userId
            select r.Name
        ).ToListAsync(cancellationToken);

        return [.. roles];
    }
}
