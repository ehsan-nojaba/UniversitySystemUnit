using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.StudentPreRegistration.DTOs;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Queries.GetStudentPreRegistration;

public sealed class GetStudentPreRegistrationQueryHandler
    : IRequestHandler<GetStudentPreRegistrationQuery, StudentPreRegistrationDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentPreRegistrationQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<StudentPreRegistrationDto?> Handle(
        GetStudentPreRegistrationQuery request,
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

        // 2. Fetch pre-registration with items and course details
        var preRegistration = await _context.StudentPreRegistrations
            .AsNoTracking()
            .Include(pr => pr.Items)
                .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(
                pr => pr.StudentId == student.Id && pr.AcademicTermId == request.AcademicTermId,
                cancellationToken);

        if (preRegistration is null)
            return null;

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
