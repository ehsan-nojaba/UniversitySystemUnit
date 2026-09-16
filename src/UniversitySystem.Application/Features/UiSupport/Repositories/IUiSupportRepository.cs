using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Repositories;
/// <summary>قرارداد خواندن اطلاعات راه‌اندازی فرم‌ها و حساب جاری؛ کوئری EF در Persistence اجرا می‌شود.</summary>
public interface IUiSupportRepository
{
    Task<IReadOnlyCollection<AcademicTermOptionDto>> GetTermsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<CourseOptionDto>> GetCoursesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProfessorOptionDto>> GetProfessorsAsync(CancellationToken cancellationToken);
    Task<CurrentUserDto?> GetCurrentUserAsync(long userId, CancellationToken cancellationToken);
}
