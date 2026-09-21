using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetActiveCourses;
/// <summary>درخواست خواندن GetActiveCourses برای راه‌اندازی UI.</summary>
public sealed record GetActiveCoursesQuery(long? MajorId = null, bool IncludeAll = false) : IRequest<ICollection<CourseOptionDto>>;
