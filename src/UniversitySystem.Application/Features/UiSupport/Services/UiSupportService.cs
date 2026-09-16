using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Repositories;

namespace UniversitySystem.Application.Features.UiSupport.Services;
/// <summary>فهرست‌های فرم و پروفایل حساب جاری را فراهم می‌کند؛ مالک حساب از هویت احراز‌شده تعیین می‌شود.</summary>
public sealed class UiSupportService(IUiSupportRepository repository, ICurrentUserService currentUser)
{
    public Task<IReadOnlyCollection<AcademicTermOptionDto>> GetTermsAsync(CancellationToken cancellationToken) => repository.GetTermsAsync(cancellationToken);
    public Task<IReadOnlyCollection<CourseOptionDto>> GetCoursesAsync(CancellationToken cancellationToken) => repository.GetCoursesAsync(cancellationToken);
    public Task<IReadOnlyCollection<ProfessorOptionDto>> GetProfessorsAsync(CancellationToken cancellationToken) => repository.GetProfessorsAsync(cancellationToken);
    public async Task<CurrentUserDto> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        if (!long.TryParse(currentUser.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("Authenticated user is required.");
        }

        return await repository.GetCurrentUserAsync(userId, cancellationToken) ?? throw new UnauthorizedAccessException("User account is missing or inactive.");
    }
}
