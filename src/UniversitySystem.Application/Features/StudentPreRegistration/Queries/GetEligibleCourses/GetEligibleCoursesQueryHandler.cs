using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetEligibleCourses;

/// <summary>
/// Handles retrieving eligible courses for a student in pre-registration:
/// 1. Resolves Student entity from current authenticated user context.
/// 2. Verifies academic term existence.
/// 3. Resolves the active Curriculum for student's Major.
/// 4. Filters out courses already passed (from StudentCourseHistory or completed Enrollment).
/// 5. Verifies all prerequisites for candidate courses are met (passed).
/// 6. Returns sorted eligible courses with prerequisite details.
/// </summary>
public sealed class GetEligibleCoursesQueryHandler
    : IRequestHandler<GetEligibleCoursesQuery, ICollection<EligibleCourseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStudentCourseEligibilityService _eligibilityService;

    public GetEligibleCoursesQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStudentCourseEligibilityService eligibilityService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _eligibilityService = eligibilityService;
    }

    public async Task<ICollection<EligibleCourseDto>> Handle(
        GetEligibleCoursesQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Resolve current user
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId) ||
            !long.TryParse(_currentUserService.UserId, out var userId))
            throw new UnauthorizedAccessException("کاربر جاری احراز هویت نشده است.");

        // 2. Resolve student profile
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (student is null)
            throw new NotFoundException("پروفایل دانشجویی برای کاربر جاری یافت نشد.");

        // 3. Verify academic term
        var termExists = await _context.AcademicTerms
            .AsNoTracking()
            .AnyAsync(t => t.Id == request.AcademicTermId, cancellationToken);

        if (!termExists)
            throw new NotFoundException(nameof(AcademicTerm), request.AcademicTermId);

        // 4. Delegate to eligibility service
        return await _eligibilityService.GetEligibleCoursesAsync(
            student.Id,
            request.AcademicTermId,
            cancellationToken);
    }
}
