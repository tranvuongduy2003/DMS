using System.Data;

namespace DMS.Domain.Common.Persistence;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
