using Microsoft.EntityFrameworkCore;
using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

/// <summary>
/// دسترسی EF به پروفایل دانشجو، ترم و درخواست پیش‌انتخاب با درس‌های آن؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class StudentPreRegistrationRepository(ApplicationDbContext context) : IStudentPreRegistrationRepository
{
    public async Task<Student?> GetStudentByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await (
            from s in context.Students.AsNoTracking()
            where s.UserId == userId
            select s
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await (
            from t in context.AcademicTerms.AsNoTracking()
            where t.Id == termId
            select t
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAsync(long studentId, long termId, CancellationToken cancellationToken = default)
    {
        return await context.StudentPreRegistrations
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.StudentId == studentId && pr.AcademicTermId == termId, cancellationToken);
    }

    public async Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAndCoursesAsync(long studentId, long termId, CancellationToken cancellationToken = default)
    {
        return await context.StudentPreRegistrations
            .AsNoTracking()
            .Include(pr => pr.Items)
            .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(pr => pr.StudentId == studentId && pr.AcademicTermId == termId, cancellationToken);
    }

    public void AddPreRegistration(StudentPreRegistrationEntity preRegistration) => context.StudentPreRegistrations.Add(preRegistration);
}
