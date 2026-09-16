using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Services;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetActiveCourses;
/// <summary>تحویل درخواست خواندن اطلاعات UI به سرویس، بدون وابستگی به EF.</summary>
public sealed class GetActiveCoursesQueryHandler(UiSupportService service) : IRequestHandler<GetActiveCoursesQuery, IReadOnlyCollection<CourseOptionDto>>
{
    public Task<IReadOnlyCollection<CourseOptionDto>> Handle(GetActiveCoursesQuery request, CancellationToken cancellationToken) => service.GetCoursesAsync(cancellationToken);
}
