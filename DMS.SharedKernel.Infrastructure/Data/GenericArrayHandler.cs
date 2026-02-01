using System.Data;
using Dapper;

namespace DMS.SharedKernel.Infrastructure.Data;

internal sealed class GenericArrayHandler<T> : SqlMapper.TypeHandler<T[]>
{
    public override void SetValue(IDbDataParameter parameter, T[]? value)
    {
        parameter.Value = value;
    }

    public override T[]? Parse(object value)
    {
        return value as T[];
    }
}
