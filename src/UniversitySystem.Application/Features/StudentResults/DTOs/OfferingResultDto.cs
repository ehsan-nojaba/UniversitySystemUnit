using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.DTOs;

/// <summary>
/// وضعیت یک ارائه برای دانشجو: ظرفیت کل و باقی‌مانده، فعالیت، استادهای تخصیص‌یافته و برنامه کلاس.
/// </summary>
public sealed record OfferingResultDto(long CourseOfferingId, int Capacity, int RemainingCapacity, bool IsActive,
    IReadOnlyCollection<ProfessorResultDto> Professors, IReadOnlyCollection<ScheduleResultDto> Schedule);
