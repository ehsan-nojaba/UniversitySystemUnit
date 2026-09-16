using Microsoft.EntityFrameworkCore;
using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به پروفایل دانشجو، ترم و درخواست پیش‌انتخاب با درس‌های آن؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class StudentPreRegistrationRepository(ApplicationDbContext _context) : IStudentPreRegistrationRepository
{
    public async Task<Student?> GetStudentByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
    }

    public async Task<AcademicTerm?> GetAcademicTermAsync(long termId, CancellationToken cancellationToken = default)
    {
        return await _context.AcademicTerms.AsNoTracking().FirstOrDefaultAsync(t => t.Id == termId, cancellationToken);
    }

    public async Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAsync(long studentId, long termId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentPreRegistrations.Include(pr => pr.Items).FirstOrDefaultAsync(pr => pr.StudentId == studentId && pr.AcademicTermId == termId, cancellationToken);
    }

    public async Task<StudentPreRegistrationEntity?> GetPreRegistrationWithItemsAndCoursesAsync(long studentId, long termId, CancellationToken cancellationToken = default)
    {
        // این Aggregate در ارسال نهایی تغییر می‌کند؛ باید توسط EF رهگیری شود.
        return await _context.StudentPreRegistrations.Include(pr => pr.Items).ThenInclude(i => i.Course).FirstOrDefaultAsync(pr => pr.StudentId == studentId && pr.AcademicTermId == termId, cancellationToken);
    }

    public void AddPreRegistration(StudentPreRegistrationEntity preRegistration) => _context.StudentPreRegistrations.Add(preRegistration);
}
