using MediatR;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.Services;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Queries.GetCourseDemandSummary;

/// <summary>
/// درخواست «دریافت تقاضای درس از درخواست‌های ارسال‌شده» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetCourseDemandSummaryQueryHandler(IAdminPreRegistrationService preRegistrationService)
    : IRequestHandler<GetCourseDemandSummaryQuery, ICollection<CourseDemandDto>>
{
    public Task<ICollection<CourseDemandDto>> Handle(GetCourseDemandSummaryQuery request, CancellationToken cancellationToken)
        => preRegistrationService.GetCourseDemandSummaryAsync(request.AcademicTermId, cancellationToken);
}
