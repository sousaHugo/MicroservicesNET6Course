
using Catalog.Domain.Entities;

namespace Catalog.Application.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(string Id);
    Task<IEnumerable<Product>> GetByNameAsync(string Name);
    Task<IEnumerable<Product>> GetByCategoryAsync(string Category);
    Task Create(Product Product);
    Task<bool> Update(Product Product);
    Task<bool> Delete(string Id);
    Task<bool> Exists(string Id);
}
