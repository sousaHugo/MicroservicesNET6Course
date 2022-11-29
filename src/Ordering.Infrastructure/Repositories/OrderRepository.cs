using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext DbContext) : base(DbContext)
    {
    }

    public async Task<IEnumerable<Order>> GetOrderByUsername(string Username)
    {
        return await _dbContext.Orders
            .Where(a => a.UserName == Username)
            .ToListAsync();
    }
}
