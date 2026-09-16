using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

/// <summary>
/// ورودی و خروجی زمان آزاد استاد شامل روز هفته، ساعت شروع و پایان؛ زیرمجموعه درخواست تدریس است.
/// </summary>
public sealed record AvailabilityInput(DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime);
