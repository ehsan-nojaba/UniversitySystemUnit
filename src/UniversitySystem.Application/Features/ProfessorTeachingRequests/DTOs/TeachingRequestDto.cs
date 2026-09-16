using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

/// <summary>
/// پاسخ درخواست تدریس: شناسه درخواست، ترم، وضعیت، زمان ارسال، درس‌ها و بازه‌های آزاد استاد.
/// </summary>
public sealed record TeachingRequestDto(long ProfessorTeachingRequestId, long AcademicTermId, string Status,
    DateTime? SubmittedAt, IReadOnlyCollection<TeachingCourseDto> Courses, IReadOnlyCollection<AvailabilityInput> Availability);
