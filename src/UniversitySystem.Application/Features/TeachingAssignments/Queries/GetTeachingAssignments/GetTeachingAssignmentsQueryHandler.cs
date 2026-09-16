using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Services;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

/// <summary>
/// درخواست «مشاهده استادهای تخصیص‌یافته» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetTeachingAssignmentsQueryHandler(ITeachingAssignmentService service)
    : IRequestHandler<GetTeachingAssignmentsQuery, ICollection<TeachingAssignmentDto>>
{
    public Task<ICollection<TeachingAssignmentDto>> Handle(GetTeachingAssignmentsQuery request, CancellationToken cancellationToken)
        => service.GetAssignmentsAsync(request.CourseOfferingId, cancellationToken);
}
