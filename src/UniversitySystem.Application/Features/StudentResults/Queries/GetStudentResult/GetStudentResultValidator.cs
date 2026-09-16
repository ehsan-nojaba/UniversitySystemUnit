using FluentValidation;

namespace UniversitySystem.Application.Features.StudentResults.Queries.GetStudentResult;

/// <summary>
/// اعتبارسنجی ورودی عملیات «دریافت نتیجه ارائه درس‌های پیش‌انتخاب» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class GetStudentResultValidator : AbstractValidator<GetStudentResultQuery>
{
    public GetStudentResultValidator() => RuleFor(x => x.AcademicTermId).GreaterThan(0);
}
