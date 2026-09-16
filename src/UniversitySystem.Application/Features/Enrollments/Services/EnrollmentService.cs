using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.Enrollments.Services;
/// <summary>
/// ثبت‌نام قطعی و مشاهده ثبت‌نام دانشجو را هماهنگ می‌کند؛ داخل تراکنش، شرایط آموزشی، ظرفیت، تکرار و تداخل را کنترل می‌کند و سپس Enrollment می‌سازد.
/// </summary>
public sealed class EnrollmentService(IEnrollmentRepository repository, IStudentPreRegistrationRepository studentRepository,
    ICurrentUserService currentUser, IStudentCourseEligibilityService eligibility, IDateTimeProvider clock, IUnitOfWork unitOfWork)
{
    private async Task<Student> GetStudentAsync(CancellationToken cancellationToken)
    {
        if (!long.TryParse(currentUser.UserId, out var id)) throw new UnauthorizedAccessException();
        return await studentRepository.GetStudentByUserIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Student profile was not found.");
    }
    public async Task<IReadOnlyCollection<EnrollmentDto>> GetAsync(long termId, CancellationToken cancellationToken)
    {
        var student = await GetStudentAsync(cancellationToken);
        _ = await studentRepository.GetAcademicTermAsync(termId, cancellationToken) ?? throw new NotFoundException(nameof(AcademicTerm), termId);
        return (await repository.GetStudentEnrollmentsAsync(student.Id, termId, cancellationToken)).Select(Map).ToList();
    }
    public async Task<EnrollmentDto> CreateAsync(long offeringId, CancellationToken cancellationToken)
    {
        var student = await GetStudentAsync(cancellationToken);
        return await repository.ExecuteSerializableAsync(async () =>
        {
            var offering = await repository.GetOfferingAsync(offeringId, cancellationToken)
                ?? throw new NotFoundException(nameof(CourseOffering), offeringId);
            if (!offering.IsActive || !offering.Course.IsActive || !offering.AcademicTerm.IsActive)
                throw new BusinessException("Course, offering and academic term must be active.");
            var registration = await studentRepository.GetPreRegistrationWithItemsAsync(student.Id, offering.AcademicTermId, cancellationToken);
            if (registration is null || registration.Status != RequestStatus.Submitted
                || !registration.Items.Any(i => i.CourseId == offering.CourseId))
                throw new BusinessException("Course must be in the student's submitted pre-registration.");
            var eligible = await eligibility.GetEligibleCoursesAsync(student.Id, offering.AcademicTermId, cancellationToken);
            if (!eligible.Any(c => c.CourseId == offering.CourseId)) throw new BusinessException("Course is no longer eligible.");
            var enrollments = await repository.GetStudentEnrollmentsAsync(student.Id, offering.AcademicTermId, cancellationToken);
            if (enrollments.Any(e => e.CourseOfferingId == offeringId
                || (e.CourseOffering.CourseId == offering.CourseId && e.Status == EnrollmentStatus.Enrolled)))
                throw new BusinessException("Student is already registered for this course or offering.");
            if (await repository.GetEnrollmentCountAsync(offeringId, cancellationToken) >= offering.Capacity)
                throw new BusinessException("Offering has no remaining capacity.");
            if (enrollments.Where(e => e.Status == EnrollmentStatus.Enrolled).SelectMany(e => e.CourseOffering.Schedules)
                .Any(s => offering.Schedules.Any(n => s.DayOfWeek == n.DayOfWeek && s.StartTime < n.EndTime && n.StartTime < s.EndTime)))
                throw new BusinessException("Offering conflicts with the student's schedule.");
            var enrollment = new Enrollment(student.Id, offeringId, clock.UtcNow);
            repository.Add(enrollment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new EnrollmentDto(enrollment.Id, offeringId, offering.AcademicTermId, offering.CourseId,
                offering.Course.Code, offering.Course.Title, enrollment.Status.ToString(), enrollment.EnrolledAt);
        }, cancellationToken);
    }
    private static EnrollmentDto Map(Enrollment e) => new(e.Id, e.CourseOfferingId, e.CourseOffering.AcademicTermId,
        e.CourseOffering.CourseId, e.CourseOffering.Course.Code, e.CourseOffering.Course.Title, e.Status.ToString(), e.EnrolledAt);
}
