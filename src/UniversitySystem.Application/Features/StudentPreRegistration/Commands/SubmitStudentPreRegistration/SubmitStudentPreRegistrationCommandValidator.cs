using FluentValidation;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SubmitStudentPreRegistration;

/// <summary>
/// Validator for <see cref="SubmitStudentPreRegistrationCommand"/>.
/// </summary>
public sealed class SubmitStudentPreRegistrationCommandValidator
    : AbstractValidator<SubmitStudentPreRegistrationCommand>
{
    public SubmitStudentPreRegistrationCommandValidator()
    {
        RuleFor(x => x.AcademicTermId)
            .GreaterThan(0)
            .WithMessage("شناسه ترم تحصیلی نامعتبر است.");
    }
}
