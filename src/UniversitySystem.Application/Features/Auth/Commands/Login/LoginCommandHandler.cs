using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Interfaces;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handles user login:
/// 1. Finds user by username.
/// 2. Verifies user exists and is active.
/// 3. Verifies password against stored password hash.
/// 4. Retrieves user roles.
/// 5. Issues signed JWT access token.
/// </summary>
public sealed class LoginCommandHandler(IApplicationDbContext _context , IPasswordHasher _passwordHasher , ITokenService _tokenService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == normalizedUsername, cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");
        }

        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("نام کاربری یا کلمه عبور نادرست است.");
        }

        var roleNames = await _context.UserRoles.Where(ur => ur.UserId == user.Id) 
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToListAsync(cancellationToken);

        var (accessToken, expiresAt) = _tokenService.GenerateToken(user, roleNames);

        return new LoginResponse(
            AccessToken: accessToken,
            ExpiresAt: expiresAt,
            UserId: user.Id,
            FullName: user.FullName,
            Roles: roleNames
        );
    }
}
