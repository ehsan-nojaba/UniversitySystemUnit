using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.DTOs;

/// <summary>
/// روز و بازه برگزاری کلاس در نتیجه پیش‌انتخاب؛ اطلاعات برنامه واقعی ارائه را نمایش می‌دهد.
/// </summary>
public sealed record ScheduleResultDto(DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime);
