namespace UniversitySystem.Application.Features.Enrollments.DTOs;
/// <summary>
/// پاسخ ثبت‌نام قطعی شامل شناسه ثبت‌نام و ارائه، ترم، مشخصات درس، وضعیت و زمان ثبت‌نام.
/// </summary>
public sealed record EnrollmentDto(long EnrollmentId, long CourseOfferingId, long AcademicTermId,
    long CourseId, string CourseCode, string CourseTitle, string Status, DateTime EnrolledAt);
