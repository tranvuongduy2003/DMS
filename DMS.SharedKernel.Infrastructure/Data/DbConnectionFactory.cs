using System.Data.Common;
using DMS.SharedKernel.Application.Data;
using Npgsql;

namespace DMS.SharedKernel.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
