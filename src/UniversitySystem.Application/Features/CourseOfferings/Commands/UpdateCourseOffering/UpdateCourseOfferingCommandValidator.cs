using FluentValidation;

namespace UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;

public sealed class UpdateCourseOfferingCommandValidator : AbstractValidator<UpdateCourseOfferingCommand>
{
    public UpdateCourseOfferingCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("شناسه ارائه درس نامعتبر است.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("ظرفیت کلاس باید یک عدد مثبت باشد.");
    }
}
