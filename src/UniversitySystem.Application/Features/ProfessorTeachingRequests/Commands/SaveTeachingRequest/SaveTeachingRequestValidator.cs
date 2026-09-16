using FluentValidation;

namespace UniversitySystem.Application.Features.ProfessorTeachingRequests.Commands.SaveTeachingRequest;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ذخیره یا ویرایش درس‌های درخواست تدریس» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SaveTeachingRequestValidator : AbstractValidator<SaveTeachingRequestCommand>
{
    public SaveTeachingRequestValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0);
        RuleFor(x => x.Courses).NotNull().Must(c => c is null || c.Select(x => x.CourseId).Distinct().Count() == c.Count).WithMessage("Duplicate courses are not allowed.");
        RuleForEach(x => x.Courses).ChildRules(c => { c.RuleFor(x => x.CourseId).GreaterThan(0); c.RuleFor(x => x.Priority).GreaterThan(0); });
    }
}
