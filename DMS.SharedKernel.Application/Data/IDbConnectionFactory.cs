using System.Data.Common;

namespace DMS.SharedKernel.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
