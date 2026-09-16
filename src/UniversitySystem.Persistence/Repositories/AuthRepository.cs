using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.Auth.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به حساب کاربر و نقش‌های او برای ورود؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class AuthRepository(ApplicationDbContext _context) : IAuthRepository
{
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<string[]> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default)
    {
        var roles = await (from ur in _context.UserRoles.AsNoTracking() join r in _context.Roles.AsNoTracking() on ur.RoleId equals r.Id where ur.UserId == userId select r.Name).ToListAsync(cancellationToken);
        return[..roles];
    }
}
