using UniversitySystem.Application.Features.ProfessorTeachingRequests.DTOs;

namespace UniversitySystem.Api.Contracts;

/// <summary>
/// بدنه HTTP انتخاب درس‌های پیشنهادی استاد؛ شامل شناسه درس و اولویت تدریس است.
/// </summary>
public sealed record SaveProfessorTeachingRequestRequest(ICollection<TeachingCourseInput> Courses);
