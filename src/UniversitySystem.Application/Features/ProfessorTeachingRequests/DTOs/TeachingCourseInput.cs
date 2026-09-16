using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

/// <summary>
/// ورودی انتخاب درس استاد شامل شناسه درس و اولویت تدریس؛ تخصیص نهایی استاد به کلاس نیست.
/// </summary>
public sealed record TeachingCourseInput(long CourseId, int Priority);
