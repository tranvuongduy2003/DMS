using System.Linq.Expressions;
using DMS.Domain.Common.Entities;

namespace DMS.Domain.Common.Persistence;

public interface IRepositoryBase<T> where T : EntityBase
{
    Task Update(T entity);

    IQueryable<T> FindAll(bool trackChanges = false);

    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    Task<bool> ExistAsync(Guid id);

    Task<bool> ExistAsync(Expression<Func<T, bool>> expression);

    Task<T> GetByIdAsync(Guid id);

    Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);

    Task CreateAsync(T entity);

    Task CreateListAsync(IEnumerable<T> entities);

    Task Delete(T entity);

    Task DeleteList(IEnumerable<T> entities);

    Task SoftDelete(T entity);

    Task Restore(T entity);
}
