namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// خطای نداشتن اجازه انجام عملیات؛ در API به پاسخ ۴۰۳ تبدیل می‌شود.
/// </summary>
public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base("You do not have permission to perform this action.")
    {
    }
}
