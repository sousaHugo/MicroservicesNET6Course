using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly OrderContext _dbContext;

    public RepositoryBase(OrderContext DbContext)
    {
        _dbContext = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
    }
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate)
    {
        return await _dbContext.Set<T>().Where(Predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string IncludeString = null, bool DisableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (DisableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(IncludeString)) query = query.Include(IncludeString);

        if (Predicate != null) query = query.Where(Predicate);

        if (OrderBy != null)
            return await OrderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, List<Expression<Func<T, object>>> Includes = null, bool DisableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (DisableTracking) query = query.AsNoTracking();

        if (Includes != null) query = Includes.Aggregate(query, (current, include) => current.Include(include));

        if (Predicate != null) query = query.Where(Predicate);

        if (OrderBy != null)
            return await OrderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int Id)
    {
        return await _dbContext.Set<T>().FindAsync(Id);
    }
    public async Task<T> AddAsync(T Entity)
    {
        _dbContext.Set<T>().Add(Entity);
        await _dbContext.SaveChangesAsync();
        return Entity;
    }
    public async Task UpdateAsync(T Entity)
    {
        _dbContext.Entry(Entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(T Entity)
    {
        _dbContext.Set<T>().Remove(Entity);
        await _dbContext.SaveChangesAsync();
    }
}
