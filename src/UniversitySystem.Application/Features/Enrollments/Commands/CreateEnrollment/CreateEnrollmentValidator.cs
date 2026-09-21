using FluentValidation;
using UniversitySystem.Application.Features.Enrollments.DTOs;
using UniversitySystem.Application.Features.Enrollments.Services;

namespace UniversitySystem.Application.Features.Enrollments.Commands.CreateEnrollment;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ثبت‌نام قطعی دانشجو در ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentCommand>
{
    public CreateEnrollmentValidator()
    {
        RuleFor(x => x.CourseOfferingId).GreaterThan(0);
    }
}
