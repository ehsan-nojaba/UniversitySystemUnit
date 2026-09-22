namespace UniversitySystem.Application.Common.Exceptions;

/// <summary>
/// خطای پایان سهمیه پیش‌انتخاب؛ API آن را با کد 409 به کلاینت برمی‌گرداند تا ورود به صفحه متوقف شود.
/// </summary>
public class PreRegistrationLimitException : Exception
{
    public PreRegistrationLimitException(string message)
        : base(message)
    {
    }
}
