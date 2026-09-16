using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Application.Features.StudentResults.DTOs;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.Services;
/// <summary>
/// برای درس‌های پیش‌انتخاب ارسال‌شده دانشجو، ارائه‌ها، استادها، برنامه و ظرفیت باقی‌مانده را جمع می‌کند؛ پیش‌انتخاب را به ثبت‌نام تبدیل نمی‌کند.
/// </summary>
public sealed class StudentResultService(IStudentPreRegistrationRepository studentRepository, ICourseOfferingRepository offeringRepository, IEnrollmentRepository enrollmentRepository, ICurrentUserService currentUser)
{
    public async Task<StudentResultDto> GetAsync(long termId, CancellationToken cancellationToken)
    {
        if (!long.TryParse(currentUser.UserId, out var id))
        {
            throw new UnauthorizedAccessException();
        }

        var student = await studentRepository.GetStudentByUserIdAsync(id, cancellationToken) ?? throw new NotFoundException("Student profile was not found.");
        _ = await studentRepository.GetAcademicTermAsync(termId, cancellationToken) ?? throw new NotFoundException(nameof(AcademicTerm), termId);
        var registration = await studentRepository.GetPreRegistrationWithItemsAndCoursesAsync(student.Id, termId, cancellationToken) ?? throw new NotFoundException("Pre-registration was not found.");
        if (registration.Status != RequestStatus.Submitted)
        {
            throw new BusinessException("Pre-registration must be submitted.");
        }

        var allOfferings = await offeringRepository.GetOfferingsByTermAsync(termId, cancellationToken);
        var results = new List<CourseResultDto>();
        foreach (var item in registration.Items.OrderBy(i => i.Priority))
        {
            var offerings = new List<OfferingResultDto>();
            foreach (var candidate in allOfferings.Where(o => o.CourseId == item.CourseId))
            {
                var offering = (await enrollmentRepository.GetOfferingAsync(candidate.CourseOfferingId, cancellationToken))!;
                var count = await enrollmentRepository.GetEnrollmentCountAsync(offering.Id, cancellationToken);
                offerings.Add(new(offering.Id, offering.Capacity, Math.Max(0, offering.Capacity - count), offering.IsActive, offering.TeachingAssignments.Select(a => new ProfessorResultDto(a.ProfessorId, a.Professor.User.FirstName + " " + a.Professor.User.LastName)).ToList(), offering.Schedules.OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).Select(s => new ScheduleResultDto(s.DayOfWeek, s.StartTime, s.EndTime)).ToList()));
            }

            results.Add(new(item.CourseId, item.Course.Code, item.Course.Title, item.Priority, offerings.Any(o => o.IsActive), offerings));
        }

        return new(termId, registration.Status.ToString(), results);
    }
}
