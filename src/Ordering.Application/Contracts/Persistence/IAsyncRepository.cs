using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : EntityBase
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
                                    string IncludeString = null,
                                    bool DisableTracking = true);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
                                    List<Expression<Func<T, object>>> Includes = null,
                                    bool DisableTracking = true);
    Task<T> GetByIdAsync(int Id);
    Task<T> AddAsync(T Entity);
    Task UpdateAsync(T Entity);
    Task DeleteAsync(T Entity);
}
