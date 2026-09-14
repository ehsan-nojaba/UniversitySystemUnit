using MediatR;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

public class UpdateCourseOfferingCommand : IRequest<CourseOfferingDto>
{
    public long Id { get; set; }
    public int Capacity { get; set; }
    public bool? IsActive { get; set; }
}
