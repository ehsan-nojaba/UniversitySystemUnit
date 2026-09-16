using FluentValidation;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// اعتبارسنجی ورودی عملیات «ورود کاربر و دریافت توکن» پیش از اجرای منطق؛ قواعد وابسته به داده در سرویس بررسی می‌شوند.
/// </summary>
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("نام کاربری الزامی است.")
            .MaximumLength(100)
            .WithMessage("نام کاربری نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("کلمه عبور الزامی است.")
            .MaximumLength(200)
            .WithMessage("کلمه عبور نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");
    }
}
