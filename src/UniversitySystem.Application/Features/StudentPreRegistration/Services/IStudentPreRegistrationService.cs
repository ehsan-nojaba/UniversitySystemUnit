using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Services;

public interface IStudentPreRegistrationService
{
    Task<StudentPreRegistrationDto> SaveDraftAsync(long academicTermId, ICollection<SelectedCourseItemDto> courses, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationDto> SubmitAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationDto?> GetByTermAsync(long academicTermId, CancellationToken cancellationToken = default);
    Task<ICollection<EligibleCourseDto>> GetEligibleCoursesAsync(long academicTermId, CancellationToken cancellationToken = default);
}
