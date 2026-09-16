using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;

/// <summary>
/// اعتبارسنجی ورودی عملیات «حذف تخصیص استاد» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class RemoveProfessorAssignmentCommandValidator : AbstractValidator<RemoveProfessorAssignmentCommand>
{
    public RemoveProfessorAssignmentCommandValidator()
    {
        RuleFor(x => x.CourseOfferingId)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");

        RuleFor(x => x.ProfessorId)
            .GreaterThan(0)
            .WithMessage("شناسه استاد نامعتبر است.");
    }
}
