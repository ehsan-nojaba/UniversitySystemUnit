using FluentValidation;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Queries.GetStudentEnrollments;

/// <summary>
/// اعتبارسنجی ورودی عملیات «مشاهده ثبت‌نام‌های دانشجو در ترم» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetStudentEnrollmentsValidator : AbstractValidator<GetStudentEnrollmentsQuery>
{
    public GetStudentEnrollmentsValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
    }
}
