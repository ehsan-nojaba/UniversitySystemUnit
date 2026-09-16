using FluentValidation;

namespace UniversitySystem.Application.Features.StudentPreRegistration.Commands.SaveStudentPreRegistration;
/// <summary>
/// اعتبارسنجی ورودی عملیات «ذخیره یا ویرایش پیش‌نویس پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class SaveStudentPreRegistrationCommandValidator : AbstractValidator<SaveStudentPreRegistrationCommand>
{
    public SaveStudentPreRegistrationCommandValidator()
    {
        RuleFor(x => x.AcademicTermId).GreaterThan(0).WithMessage("شناسه ترم تحصیلی نامعتبر است.");
        RuleFor(x => x.Courses).NotNull().WithMessage("لیست درس‌ها نمی‌تواند خالی باشد.").Must(c => c != null && c.Count > 0).WithMessage("حداقل یک درس باید برای پیش‌ثبت‌نام انتخاب شود.").Must(HaveUniqueCourseIds).WithMessage("انتخاب درس‌های تکراری مجاز نیست.");
        RuleForEach(x => x.Courses).ChildRules(course =>
        {
            course.RuleFor(c => c.CourseId).GreaterThan(0).WithMessage("شناسه درس نامعتبر است.");
            course.RuleFor(c => c.Priority).GreaterThan(0).WithMessage("اولویت درس باید بزرگتر از صفر باشد.");
        });
    }

    private static bool HaveUniqueCourseIds(ICollection<DTOs.SelectedCourseItemDto>? courses)
    {
        if (courses is null || courses.Count <= 1)
        {
            return true;
        }

        return courses.Select(c => c.CourseId).Distinct().Count() == courses.Count;
    }
}
