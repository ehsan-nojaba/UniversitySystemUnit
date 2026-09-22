namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// خطای پیدا نشدن اطلاعات درخواستی؛ در API به پاسخ ۴۰۴ تبدیل می‌شود.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string resourceName, object key) : base($"اطلاعات موردنظر با شناسه «{key}» پیدا نشد.")
    {
    }
}
