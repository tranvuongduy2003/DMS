using DMS.Domain.Common.Entities;

namespace DMS.Domain.Common.Persistence;

public interface ICachedRepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
}
