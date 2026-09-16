using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

/// <summary>
/// تغییرات ریپازیتوری‌های همان درخواست را با DbContext مشترک ذخیره می‌کند؛ منطق آموزشی ندارد.
/// </summary>
public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}
