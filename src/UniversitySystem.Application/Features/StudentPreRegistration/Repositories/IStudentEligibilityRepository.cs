
namespace UniversitySystem.Application.Features.StudentPreRegistration.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «پیش‌انتخاب واحد دانشجو»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IStudentEligibilityRepository
{
    Task<EligibilityData> GetDataAsync(long studentId, CancellationToken cancellationToken);
}
