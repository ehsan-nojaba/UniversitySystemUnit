using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Application.Features.CourseOfferings.Repositories;
using UniversitySystem.Application.Features.Enrollments.Repositories;
using UniversitySystem.Application.Features.StudentPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Features.StudentResults.DTOs;

/// <summary>
/// پاسخ نتیجه پیش‌انتخاب؛ ترم، وضعیت درخواست و نتیجه ارائه هر درس درخواستی را جمع می‌کند.
/// </summary>
public sealed record StudentResultDto(long AcademicTermId, string Status, IReadOnlyCollection<CourseResultDto> Courses);
