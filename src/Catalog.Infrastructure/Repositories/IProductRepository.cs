
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(string Id);
    Task<IEnumerable<Product>> GetByNameAsync(string Name);
    Task<IEnumerable<Product>> GetByCategoryAsync(string Category);
    Task Create(Product Product);
    Task<bool> Update(Product Product);
    Task<bool> Delete(string Id);
}
