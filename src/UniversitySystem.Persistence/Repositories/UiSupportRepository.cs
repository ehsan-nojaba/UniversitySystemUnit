using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Repositories;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>کوئری‌های خواندنی اطلاعات پایه فرم‌ها و پروفایل کاربر؛ هیچ اطلاعات رمز برنمی‌گرداند.</summary>
public sealed class UiSupportRepository(ApplicationDbContext _context) : IUiSupportRepository
{
    public async Task<ICollection<AcademicTermOptionDto>> GetTermsAsync(CancellationToken cancellationToken)
    {
        return await _context.AcademicTerms.AsNoTracking().Where(t => t.IsActive).OrderByDescending(t => t.StartDate).ThenBy(t => t.Id).Select(t => new AcademicTermOptionDto(t.Id, t.Code, t.Title, t.IsActive, t.StartDate, t.EndDate)).ToListAsync(cancellationToken);
    }
    public async Task<ICollection<CourseOptionDto>> GetCoursesAsync(long? majorId, CancellationToken cancellationToken)
    {
        return await _context.Courses.AsNoTracking().Where(c => c.IsActive && (majorId == null || c.CurriculumCourses.Any(cc => cc.Curriculum.MajorId == majorId))).OrderBy(c => c.Code).Select(c => new CourseOptionDto(c.Id, c.Code, c.Title, c.Credits)).ToListAsync(cancellationToken);
    }
    public async Task<ICollection<ProfessorOptionDto>> GetProfessorsAsync(CancellationToken cancellationToken)
    {
        return await _context.Professors.AsNoTracking().Where(p => p.User.IsActive).OrderBy(p => p.User.LastName).ThenBy(p => p.Id).Select(p => new ProfessorOptionDto(p.Id, p.PersonnelCode, p.User.FirstName + " " + p.User.LastName)).ToListAsync(cancellationToken);
    }
    public async Task<CurrentUserDto?> GetCurrentUserAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking().Where(u => u.Id == userId && u.IsActive).Select(u => new { u.Id, u.Username, FullName = u.FirstName + " " + u.LastName }).SingleOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return null;
        }

        var roles = await (from assignment in _context.UserRoles.AsNoTracking() join role in _context.Roles.AsNoTracking() on assignment.RoleId equals role.Id where assignment.UserId == userId orderby role.Name select role.Name).ToListAsync(cancellationToken);
        var student = await _context.Students.AsNoTracking().Where(s => s.UserId == userId).Select(s => new StudentProfileDto(s.Id, s.StudentNumber, s.MajorId, s.Major.Title, s.EntryYear)).SingleOrDefaultAsync(cancellationToken);
        var professor = await _context.Professors.AsNoTracking().Where(p => p.UserId == userId).Select(p => new ProfessorOptionDto(p.Id, p.PersonnelCode, user.FullName)).SingleOrDefaultAsync(cancellationToken);
        return new(user.Id, user.Username, user.FullName, roles, student, professor);
    }
}
