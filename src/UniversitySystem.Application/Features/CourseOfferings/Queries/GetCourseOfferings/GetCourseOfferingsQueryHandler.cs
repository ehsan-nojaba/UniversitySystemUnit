using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Services;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

public sealed class GetCourseOfferingsQueryHandler(ICourseOfferingService service)
    : IRequestHandler<GetCourseOfferingsQuery, ICollection<CourseOfferingDto>>
{
    public Task<ICollection<CourseOfferingDto>> Handle(GetCourseOfferingsQuery request, CancellationToken cancellationToken)
        => service.GetOfferingsByTermAsync(request.AcademicTermId, cancellationToken);
}
