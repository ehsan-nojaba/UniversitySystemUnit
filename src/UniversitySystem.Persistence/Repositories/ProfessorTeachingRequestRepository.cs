using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Features.ProfessorTeachingRequests.Repositories;
using UniversitySystem.Application.Features.ProfessorTeachingRequests;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;
/// <summary>
/// دسترسی EF به پروفایل استاد، درخواست تدریس، درس‌ها و زمان‌های آزاد؛ تصمیم آموزشی در سرویس Application انجام می‌شود.
/// </summary>
public sealed class ProfessorTeachingRequestRepository(ApplicationDbContext _context) : IProfessorTeachingRequestRepository
{
    public Task<Professor?> GetProfessorAsync(long userId, CancellationToken cancellationToken) => _context.Professors.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    public Task<AcademicTerm?> GetTermAsync(long termId, CancellationToken cancellationToken) => _context.AcademicTerms.AsNoTracking().FirstOrDefaultAsync(t => t.Id == termId, cancellationToken);
    public Task<ProfessorTeachingRequest?> GetAsync(long professorId, long termId, CancellationToken cancellationToken) => _context.ProfessorTeachingRequests.Include(r => r.Courses).ThenInclude(c => c.Course).Include(r => r.Availabilities).AsSplitQuery().FirstOrDefaultAsync(r => r.ProfessorId == professorId && r.AcademicTermId == termId, cancellationToken);
    public async Task<ICollection<Course>> GetCoursesAsync(ICollection<long> ids, CancellationToken cancellationToken) => await _context.Courses.AsNoTracking().Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
    public async Task<ICollection<ProfessorTeachingRequest>> GetSubmittedAsync(long termId, CancellationToken cancellationToken) => await _context.ProfessorTeachingRequests.AsNoTracking().Include(r => r.Professor).ThenInclude(p => p.User).Include(r => r.Courses).ThenInclude(c => c.Course).Include(r => r.Availabilities).AsSplitQuery().Where(r => r.AcademicTermId == termId && r.Status == RequestStatus.Submitted).ToListAsync(cancellationToken);
    public void Add(ProfessorTeachingRequest request) => _context.ProfessorTeachingRequests.Add(request);
}
