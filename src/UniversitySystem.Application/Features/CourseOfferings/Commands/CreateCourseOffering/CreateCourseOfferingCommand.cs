using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;

public class CreateCourseOfferingCommand : IRequest<CourseOfferingDto>
{
    public long AcademicTermId { get; set; }
    public long CourseId { get; set; }
    public int Capacity { get; set; }
}
