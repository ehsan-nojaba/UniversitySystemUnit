using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;

public class GetCourseOfferingsQuery : IRequest<ICollection<CourseOfferingDto>>
{
    public long AcademicTermId { get; set; }
}
