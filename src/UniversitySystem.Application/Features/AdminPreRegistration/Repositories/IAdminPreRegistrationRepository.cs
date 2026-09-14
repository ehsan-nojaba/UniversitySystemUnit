namespace UniversitySystem.Application.Features.AdminPreRegistration.Repositories;

public class CourseDemandAggregatedModel
{
    public long CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int StudentCount { get; set; }
    public double AveragePriority { get; set; }
}

public interface IAdminPreRegistrationRepository
{
    Task<bool> AcademicTermExistsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseDemandAggregatedModel>> GetCourseDemandSummaryAsync(long termId, CancellationToken cancellationToken = default);
}
