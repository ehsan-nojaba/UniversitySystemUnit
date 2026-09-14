using UniversitySystem.Application.Common.Interfaces;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Persistence.Repositories;

public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}
