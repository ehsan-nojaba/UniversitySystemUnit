using UniversitySystem.Domain.Entities;
using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Repositories;

public interface IStudentPreRegistrationRepository
{
    Task<Student?> GetStudentByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAsync(long studentId, long termId, CancellationToken cancellationToken = default);
    Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAndCoursesAsync(long studentId, long termId, CancellationToken cancellationToken = default);
    void AddPreRegistration(StudentPreRegistrationEntity preRegistration);
}
