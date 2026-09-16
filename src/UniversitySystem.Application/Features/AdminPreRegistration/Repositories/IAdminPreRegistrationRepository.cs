namespace UniversitySystem.Application.Features.AdminPreRegistration.Repositories;

/// <summary>
/// مدل کمکی CourseDemandAggregatedModel؛ مسئولیت آن در راهنمای فارسی پروژه توضیح داده شده است.
/// </summary>
public class CourseDemandAggregatedModel
{
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int StudentCount { get; set; }
    public double AveragePriority { get; set; }
}

/// <summary>
/// قرارداد دسترسی به داده بخش «جمع‌بندی تقاضای درس دانشجویان»؛ خواندن و ثبت داده را از تصمیم‌های آموزشی سرویس جدا می‌کند.
/// </summary>
public interface IAdminPreRegistrationRepository
{
    Task<bool> AcademicTermExistsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseDemandAggregatedModel>> GetCourseDemandSummaryAsync(long termId, CancellationToken cancellationToken = default);
}
