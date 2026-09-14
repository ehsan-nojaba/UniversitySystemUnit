using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using StudentPreRegistrationEntity = UniversitySystem.Domain.Entities.StudentPreRegistration;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;

/// <summary>
/// Handles saving (creating or updating) a student's pre-registration draft.
/// Enforces:
/// - Authenticated student identity.
/// - Valid, active academic term.
/// - Course eligibility validation via <see cref="IStudentCourseEligibilityService"/>.
/// - Editable only while in Draft status.
/// - Clean aggregate mutation via <see cref="StudentPreRegistration.AddCourse"/> and <see cref="StudentPreRegistration.RemoveCourse"/>.
/// </summary>
public sealed class SaveStudentPreRegistrationCommandHandler
    : IRequestHandler<SaveStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStudentCourseEligibilityService _eligibilityService;

    public SaveStudentPreRegistrationCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStudentCourseEligibilityService eligibilityService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _eligibilityService = eligibilityService;
    }

    public async Task<StudentPreRegistrationDto> Handle(
        SaveStudentPreRegistrationCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Resolve current user & student
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId) ||
            !long.TryParse(_currentUserService.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("کاربر جاری احراز هویت نشده است.");
        }

        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (student is null)
        {
            throw new NotFoundException("پروفایل دانشجویی برای کاربر جاری یافت نشد.");
        }

        // 2. Validate academic term
        var term = await _context.AcademicTerms
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.AcademicTermId, cancellationToken);

        if (term is null)
        {
            throw new NotFoundException(nameof(AcademicTerm), request.AcademicTermId);
        }

        if (!term.IsActive)
        {
            throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");
        }

        // 3. Verify that all selected courses are eligible
        var eligibleCourses = await _eligibilityService.GetEligibleCoursesAsync(
            student.Id,
            request.AcademicTermId,
            cancellationToken);

        var eligibleMap = eligibleCourses.ToDictionary(c => c.CourseId);

        foreach (var course in request.Courses)
        {
            if (!eligibleMap.ContainsKey(course.CourseId))
            {
                throw new BusinessException($"درس با شناسه {course.CourseId} جزو درس‌های قابل انتخاب برای این ترم نیست.");
            }
        }

        // 4. Find existing pre-registration
        var preRegistration = await _context.StudentPreRegistrations
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(
                pr => pr.StudentId == student.Id && pr.AcademicTermId == request.AcademicTermId,
                cancellationToken);

        if (preRegistration is null)
        {
            // ── Create new draft ──────────────────────────────────────────────
            preRegistration = new StudentPreRegistrationEntity(student.Id, request.AcademicTermId);
            _context.StudentPreRegistrations.Add(preRegistration);

            foreach (var item in request.Courses)
            {
                preRegistration.AddCourse(item.CourseId, item.Priority);
            }
        }
        else
        {
            // ── Update existing draft ─────────────────────────────────────────
            if (preRegistration.Status != RequestStatus.Draft)
            {
                throw new BusinessException("امکان ویرایش پیش‌ثبت‌نامی که در وضعیت پیش‌نویس (Draft) نیست وجود ندارد.");
            }

            var requestedCourseIds = request.Courses.Select(c => c.CourseId).ToHashSet();

            // Remove items no longer selected
            var itemsToRemove = preRegistration.Items
                .Where(i => !requestedCourseIds.Contains(i.CourseId))
                .ToList();

            foreach (var toRemove in itemsToRemove)
            {
                preRegistration.RemoveCourse(toRemove.CourseId);
            }

            // Add or update requested items
            var existingItems = preRegistration.Items.ToDictionary(i => i.CourseId);

            foreach (var item in request.Courses)
            {
                if (existingItems.TryGetValue(item.CourseId, out var existingItem))
                {
                    if (existingItem.Priority != item.Priority)
                    {
                        preRegistration.UpdateCoursePriority(item.CourseId, item.Priority);
                    }
                }
                else
                {
                    preRegistration.AddCourse(item.CourseId, item.Priority);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        // 5. Construct response
        var responseCourses = request.Courses
            .OrderBy(c => c.Priority)
            .Select(c =>
            {
                var info = eligibleMap[c.CourseId];
                return new PreRegistrationCourseItemDto(
                    c.CourseId,
                    info.Code,
                    info.Title,
                    info.Credits,
                    c.Priority
                );
            })
            .ToList();

        int totalCredits = responseCourses.Sum(c => c.Credits);

        return new StudentPreRegistrationDto(
            preRegistration.Id,
            preRegistration.AcademicTermId,
            preRegistration.Status.ToString(),
            responseCourses,
            totalCredits
        );
    }
}
