using FluentValidation;

namespace UniversitySystem.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validator for <see cref="LoginCommand"/>.
/// Ensures required fields meet length and format constraints before handler execution.
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
