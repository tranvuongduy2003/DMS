using Microsoft.EntityFrameworkCore.Storage;

namespace DMS.Domain.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();
}
