using UniversitySystem.Application.Common.Exceptions;
using UniversitySystem.Application.Features.AdminPreRegistration.DTOs;
using UniversitySystem.Application.Features.AdminPreRegistration.Repositories;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Features.AdminPreRegistration.Services;

/// <summary>
/// اجرای قواعد و هماهنگی عملیات بخش «جمع‌بندی تقاضای درس دانشجویان»؛ داده را از ریپازیتوری می‌گیرد و تغییرات را از طریق مدل‌های دامنه انجام می‌دهد.
/// </summary>
public sealed class AdminPreRegistrationService(IAdminPreRegistrationRepository repository) : IAdminPreRegistrationService
{
    public async Task<ICollection<CourseDemandDto>> GetCourseDemandSummaryAsync(long academicTermId, CancellationToken cancellationToken = default)
    {
        var termExists = await repository.AcademicTermExistsAsync(academicTermId, cancellationToken);
        if (!termExists) throw new NotFoundException(nameof(AcademicTerm), academicTermId);

        var demandData = await repository.GetCourseDemandSummaryAsync(academicTermId, cancellationToken);

        return (
            from d in demandData
            select new CourseDemandDto
            {
                CourseId = d.CourseId,
                CourseCode = d.CourseCode,
                CourseTitle = d.CourseTitle,
                StudentCount = d.StudentCount,
                AveragePriority = Math.Round(d.AveragePriority, 2),
                TotalRequestedCredits = d.StudentCount * d.Credits
            }
        ).ToList();
    }
}
