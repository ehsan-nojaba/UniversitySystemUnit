namespace UniversitySystem.Application.Common.Interfaces;

/// <summary>
/// قرارداد ذخیره تغییرات یک درخواست؛ سرویس Application را از پیاده‌سازی EF مستقل نگه می‌دارد.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
