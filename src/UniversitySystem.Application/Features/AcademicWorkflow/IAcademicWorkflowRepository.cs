using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.AcademicWorkflow;

/// <summary>داده درس‌های مجاز استاد و تقاضای دانشجو را از ذخیره‌سازی جدا می‌کند.</summary>
public interface IAcademicWorkflowRepository
{
    Task<bool> ProfessorExistsAsync(long professorId, CancellationToken ct);
    Task<long?> GetProfessorIdAsync(long userId, CancellationToken ct);
    Task<bool> TermExistsAsync(long termId, CancellationToken ct);
    Task<ICollection<long>> GetAllowedCourseIdsAsync(long professorId, CancellationToken ct);
    Task<ICollection<ProfessorCourseOption>> GetCoursesAsync(long professorId, long termId, CancellationToken ct);
    Task<bool> CoursesExistAsync(ICollection<long> ids, CancellationToken ct);
    Task<bool> HasCommittedCoursesAsync(long professorId, ICollection<long> ids, CancellationToken ct);
    Task ReplaceCoursesAsync(long professorId, ICollection<long> ids, CancellationToken ct);
    Task<bool> HasSubmittedProposalAsync(long professorId, long courseId, long termId, CancellationToken ct);
    Task<ICollection<FinalTeachingOffering>> GetFinalScheduleAsync(long professorId, long termId, CancellationToken ct);
}

/// <summary>درس مجاز استاد با تعداد تقاضای ارسال‌شده دانشجوها در ترم انتخابی.</summary>
public sealed record ProfessorCourseOption(long CourseId, string Code, string Title, int Credits, int StudentDemandCount);

/// <summary>برنامه نهایی کلاس‌های استاد با ظرفیت و جلسه‌های تاییدشده توسط آموزش.</summary>
public sealed record FinalTeachingOffering(long CourseOfferingId, long CourseId, string Title, int Capacity, ICollection<UniversitySystem.Application.Features.CourseOfferings.DTOs.CourseOfferingScheduleSlotDto> Schedule);
