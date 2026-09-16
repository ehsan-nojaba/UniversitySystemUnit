using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

/// <summary>
/// اعتبارسنجی ورودی عملیات «تخصیص استاد به ارائه» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class AssignProfessorCommandValidator : AbstractValidator<AssignProfessorCommand>
{
    public AssignProfessorCommandValidator()
    {
        RuleFor(x => x.CourseOfferingId)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");

        RuleFor(x => x.ProfessorId)
            .GreaterThan(0)
            .WithMessage("شناسه استاد نامعتبر است.");
    }
}
