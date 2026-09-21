using MediatR;
using UniversitySystem.Application.Features.UiSupport.DTOs;
using UniversitySystem.Application.Features.UiSupport.Services;

namespace UniversitySystem.Application.Features.UiSupport.Queries.GetActiveCourses;
/// <summary>تحویل درخواست خواندن اطلاعات UI به سرویس، بدون وابستگی به EF.</summary>
public sealed class GetActiveCoursesQueryHandler(UiSupportService service) : IRequestHandler<GetActiveCoursesQuery, ICollection<CourseOptionDto>>
{
    public Task<ICollection<CourseOptionDto>> Handle(GetActiveCoursesQuery request, CancellationToken cancellationToken) => service.GetCoursesAsync(request.MajorId, cancellationToken);
}
