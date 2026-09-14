namespace UniversitySystem.Application.Features.AdminPlanning.Repositories;

public interface IAdminPlanningRepository
{
    Task<AcademicTermInfoModel?> GetAcademicTermInfoAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseDemandCountModel>> GetSubmittedStudentDemandCountsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<ProfessorCourseInterestModel>> GetSubmittedProfessorInterestsAsync(long termId, CancellationToken cancellationToken = default);
    Task<ICollection<CourseInfoModel>> GetCoursesByIdsAsync(long[] courseIds, CancellationToken cancellationToken = default);
}
