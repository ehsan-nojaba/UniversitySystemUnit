using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Repositories;
/// <summary>قرارداد خواندن اطلاعات راه‌اندازی فرم‌ها و حساب جاری؛ کوئری EF در Persistence اجرا می‌شود.</summary>
public interface IUiSupportRepository
{
    Task<ICollection<AcademicTermOptionDto>> GetTermsAsync(CancellationToken cancellationToken);
    Task<ICollection<CourseOptionDto>> GetCoursesAsync(CancellationToken cancellationToken);
    Task<ICollection<ProfessorOptionDto>> GetProfessorsAsync(CancellationToken cancellationToken);
    Task<CurrentUserDto?> GetCurrentUserAsync(long userId, CancellationToken cancellationToken);
}
