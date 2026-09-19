using System.Data;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به ارائه با اطلاعات آموزشی و ثبت‌نام‌های دانشجو و تعداد ثبت‌نام فعال؛ مرز تراکنش ثبت‌نام نیز در این پیاده‌سازی است؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class EnrollmentRepository(ApplicationDbContext _context) : IEnrollmentRepository
{
    public async Task<T> ExecuteSerializableAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var result = await operation();
        await transaction.CommitAsync(cancellationToken);
        return result;
    }

    public Task<CourseOffering?> GetOfferingAsync(long id, CancellationToken cancellationToken) => _context.CourseOfferings.Include(o => o.Course).Include(o => o.AcademicTerm).Include(o => o.TeachingAssignments).ThenInclude(a => a.Professor).ThenInclude(p => p.User).Include(o => o.Schedules).AsSplitQuery().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    public async Task<ICollection<Enrollment>> GetStudentEnrollmentsAsync(long studentId, long termId, CancellationToken cancellationToken) => await _context.Enrollments.AsNoTracking().Include(e => e.CourseOffering).ThenInclude(o => o.Course).Include(e => e.CourseOffering).ThenInclude(o => o.Schedules).Where(e => e.StudentId == studentId && e.CourseOffering.AcademicTermId == termId).ToListAsync(cancellationToken);
    public Task<int> GetEnrollmentCountAsync(long offeringId, CancellationToken cancellationToken) => _context.Enrollments.CountAsync(e => e.CourseOfferingId == offeringId && e.Status == EnrollmentStatus.Enrolled, cancellationToken);
    public void Add(Enrollment enrollment) => _context.Enrollments.Add(enrollment);
}
