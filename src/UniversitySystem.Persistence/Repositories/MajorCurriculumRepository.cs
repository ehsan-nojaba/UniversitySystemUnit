using System.Data;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.MajorCurricula;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

/// <summary>خواندن رشته‌ها و آخرین چارت فعال، ذخیره ارتباط درس‌ها و کنترل استفاده جاری دانشجویان با EF.</summary>
public sealed class MajorCurriculumRepository(ApplicationDbContext _context) : IMajorCurriculumRepository
{
    private const string CourseCodePrefix = "CE-";
    private const int FirstCourseNumber = 101;

    public async Task<ICollection<MajorOptionDto>> GetMajorsAsync(CancellationToken ct) => await _context.Majors.AsNoTracking().OrderBy(m => m.Title).ThenBy(m => m.Code).Select(m => new MajorOptionDto(m.Id, m.Code, m.Title, m.Department.Title, m.IsActive)).ToListAsync(ct);
    public Task<Major?> GetMajorAsync(long majorId, CancellationToken ct) => _context.Majors.AsNoTracking().SingleOrDefaultAsync(m => m.Id == majorId, ct);
    public Task<Curriculum?> GetCurrentAsync(long majorId, CancellationToken ct) => _context.Curriculums.Include(c => c.CurriculumCourses).ThenInclude(c => c.Course).Where(c => c.MajorId == majorId && c.IsActive).OrderByDescending(c => c.Id).FirstOrDefaultAsync(ct);
    public async Task<ICollection<Course>> GetCoursesAsync(ICollection<long> ids, CancellationToken ct) => await _context.Courses.Where(c => ids.Contains(c.Id)).ToListAsync(ct);
    public async Task<string> GetNextCourseCodeAsync(CancellationToken ct)
    {
        var codes = await _context.Courses.AsNoTracking().Where(c => c.Code.StartsWith(CourseCodePrefix)).Select(c => c.Code).ToListAsync(ct);
        var maxNumber = codes.Select(ParseCourseNumber).Where(n => n.HasValue).Select(n => n!.Value).DefaultIfEmpty(FirstCourseNumber - 1).Max();
        return $"{CourseCodePrefix}{maxNumber + 1:D3}";
    }

    private static int? ParseCourseNumber(string code) => int.TryParse(code[CourseCodePrefix.Length..], out var number) ? number : null;
    public async Task<bool> HasCurrentStudentUseAsync(long majorId, ICollection<long> courseIds, CancellationToken ct)
    {
        var requested = await _context.StudentPreRegistrationItems.AnyAsync(i => courseIds.Contains(i.CourseId) && i.StudentPreRegistration.Student.MajorId == majorId && i.StudentPreRegistration.AcademicTerm.IsActive && i.StudentPreRegistration.Status != RequestStatus.Cancelled, ct);
        return requested || await _context.Enrollments.AnyAsync(e => e.Student.MajorId == majorId && courseIds.Contains(e.CourseOffering.CourseId) && e.CourseOffering.AcademicTerm.IsActive && e.Status == EnrollmentStatus.Enrolled, ct);
    }
    public void Add(Curriculum curriculum) => _context.Curriculums.Add(curriculum);
    public void Add(Course course) => _context.Courses.Add(course);
    public async Task<T> ExecuteSerializableAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        var result = await action();
        await transaction.CommitAsync(ct);
        return result;
    }
}
