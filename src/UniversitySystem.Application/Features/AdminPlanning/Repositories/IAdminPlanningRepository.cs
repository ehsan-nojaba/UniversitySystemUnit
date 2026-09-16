namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

/// <summary>
/// قرارداد دسترسی به داده بخش «برنامه‌ریزی آموزش بر اساس تقاضا و علاقه استاد»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IAdminPlanningRepository
{
    Task<AcademicTermInfoModel?> GetAcademicTermInfoAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseDemandCountModel>> GetSubmittedStudentDemandCountsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<ProfessorCourseInterestModel>> GetSubmittedProfessorInterestsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseInfoModel>> GetCoursesByIdsAsync(long[] courseIds, CancellationToken cancellationToken = default);
}
