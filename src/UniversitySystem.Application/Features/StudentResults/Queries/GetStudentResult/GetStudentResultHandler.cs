using MediatR;
using UniversitySystem.Application.Features.StudentResults.DTOs;
using UniversitySystem.Application.Features.StudentResults.Services;

namespace UniversitySystem.Application.Features.StudentResults.Queries.GetStudentResult;

/// <summary>
/// درخواست «دریافت نتیجه ارائه درس‌های پیش‌انتخاب» را از MediatR دریافت می‌کند و به سرویس مربوط می‌سپارد؛ کوئری دیتابیس اجرا نمی‌کند.
/// </summary>
public sealed class GetStudentResultHandler(StudentResultService service) : IRequestHandler<GetStudentResultQuery, StudentResultDto>
{
    public Task<StudentResultDto> Handle(GetStudentResultQuery request, CancellationToken cancellationToken)
    {
        return service.GetAsync(request.AcademicTermId, cancellationToken);
    }
}
