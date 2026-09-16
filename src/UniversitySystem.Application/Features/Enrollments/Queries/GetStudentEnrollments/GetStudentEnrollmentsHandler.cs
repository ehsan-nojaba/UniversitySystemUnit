using MediatR;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Queries.GetStudentEnrollments;

/// <summary>
/// درخواست «مشاهده ثبت‌نام‌های دانشجو در ترم» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetStudentEnrollmentsHandler(EnrollmentService service) : IRequestHandler<GetStudentEnrollmentsQuery, IReadOnlyCollection<EnrollmentDto>>
{
    public Task<IReadOnlyCollection<EnrollmentDto>> Handle(GetStudentEnrollmentsQuery request, CancellationToken cancellationToken) => service.GetAsync(request.AcademicTermId, cancellationToken);
}
