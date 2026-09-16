using MediatR;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;

namespace UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

/// <summary>
/// درخواست خواندن اطلاعات برای «مشاهده استادهای تخصیص‌یافته»؛ هدف آن دریافت پاسخ بدون تغییر داده است.
/// </summary>
public class GetTeachingAssignmentsQuery : IRequest<ICollection<TeachingAssignmentDto>>
{
    public long CourseOfferingId { get; set; }
}
