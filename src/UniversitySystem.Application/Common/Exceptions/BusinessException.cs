namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// خطای نقض قانون آموزشی مانند ظرفیت پر یا ویرایش درخواست ارسال‌شده؛ در API به پاسخ ۴۰۰ تبدیل می‌شود.
/// </summary>
public sealed class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }
}
