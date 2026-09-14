using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// Handles the submission of a student's pre-registration draft.
/// Enforces:
/// - Authenticated student identity.
/// - Valid and active academic term.
/// - Pre-registration exists and is in Draft status.
/// - At least one course selected.
/// - Selected courses remain eligible via <see cref="IStudentCourseEligibilityService"/>.
/// - Marks status as Submitted and records current UTC timestamp via <see cref="IDateTimeProvider"/>.
/// </summary>
public sealed class SubmitStudentPreRegistrationCommandHandler
    : IRequestHandler<SubmitStudentPreRegistrationCommand, StudentPreRegistrationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStudentCourseEligibilityService _eligibilityService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SubmitStudentPreRegistrationCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStudentCourseEligibilityService eligibilityService,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _currentUserService = currentUserService;
        _eligibilityService = eligibilityService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<StudentPreRegistrationDto> Handle(
        SubmitStudentPreRegistrationCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Resolve current student
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId) ||
            !long.TryParse(_currentUserService.UserId, out var userId))
            throw new UnauthorizedAccessException("کاربر جاری احراز هویت نشده است.");

        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (student is null)
            throw new NotFoundException("پروفایل دانشجویی برای کاربر جاری یافت نشد.");

        // 2. Validate academic term
        var term = await _context.AcademicTerms
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.AcademicTermId, cancellationToken);

        if (term is null)
            throw new NotFoundException(nameof(AcademicTerm), request.AcademicTermId);

        if (!term.IsActive)
            throw new BusinessException("ترم تحصیلی انتخابی فعال نیست.");

        // 3. Find existing pre-registration
        var preRegistration = await _context.StudentPreRegistrations
            .Include(pr => pr.Items)
                .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(
                pr => pr.StudentId == student.Id && pr.AcademicTermId == request.AcademicTermId,
                cancellationToken);

        if (preRegistration is null)
            throw new NotFoundException("پیش‌ثبت‌نامی برای این ترم تحصیلی یافت نشد.");

        // 4. Ensure status is Draft
        if (preRegistration.Status != RequestStatus.Draft)
            throw new BusinessException("فقط پیش‌ثبت‌نام در وضعیت پیش‌نویس (Draft) قابل ثبت نهایی است.");

        // 5. Ensure at least one course is selected
        if (preRegistration.Items.Count == 0)
            throw new BusinessException("برای ثبت نهایی، باید حداقل یک درس انتخاب شده باشد.");

        // 6. Verify that selected courses are still eligible
        var eligibleCourses = await _eligibilityService.GetEligibleCoursesAsync(
            student.Id,
            request.AcademicTermId,
            cancellationToken);

        var eligibleMap = eligibleCourses.ToDictionary(c => c.CourseId);

        foreach (var item in preRegistration.Items)
        {
            if (!eligibleMap.ContainsKey(item.CourseId))
                throw new BusinessException($"درس '{item.Course.Title}' دیگر برای این ترم قابل انتخاب نیست.");
        }

        // 7. Submit aggregate using IDateTimeProvider.UtcNow
        preRegistration.Submit(_dateTimeProvider.UtcNow);

        await _context.SaveChangesAsync(cancellationToken);

        // 8. Construct response
        var courseItems = preRegistration.Items
            .OrderBy(i => i.Priority)
            .Select(i => new PreRegistrationCourseItemDto
            {
                CourseId = i.CourseId,
                Code = i.Course.Code,
                Title = i.Course.Title,
                Credits = i.Course.Credits,
                Priority = i.Priority
            })
            .ToList();

        int totalCredits = courseItems.Sum(c => c.Credits);

        return new StudentPreRegistrationDto
        {
            PreRegistrationId = preRegistration.Id,
            AcademicTermId = preRegistration.AcademicTermId,
            Status = preRegistration.Status.ToString(),
            Courses = courseItems,
            TotalCredits = totalCredits,
            SubmittedAt = preRegistration.SubmittedAt
        };
    }
}
