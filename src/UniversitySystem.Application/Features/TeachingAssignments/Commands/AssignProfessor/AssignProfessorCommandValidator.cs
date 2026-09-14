using FluentValidation;

namespace UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;

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
