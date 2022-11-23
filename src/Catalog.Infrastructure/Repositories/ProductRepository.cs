using Catalog.Domain.Entities;
using Catalog.Infrastructure.Context;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;
    public ProductRepository(ICatalogContext Context)
    {
        _catalogContext = Context;
    }
    public async Task Create(Product Product)
    {
        await _catalogContext.Products.InsertOneAsync(Product);
    }

    public async Task<bool> Delete(string Id)
    {
        var deleteFilter = Builders<Product>.Filter.Eq(p => p.Id, Id);

        var deletionResult = await _catalogContext.Products.DeleteOneAsync(deleteFilter);

        return deletionResult.IsAcknowledged
            && deletionResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _catalogContext.Products.Find(a => true).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string Category)
    {
        var categoryFilter = Builders<Product>.Filter.Eq(p => p.Category, Category);

        return await _catalogContext.Products.Find(categoryFilter).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(string Id)
    {
        return await _catalogContext.Products.Find(a => a.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string Name)
    {
        var nameFilter = Builders<Product>.Filter.Eq(p => p.Name, Name);

        return await _catalogContext.Products.Find(nameFilter).ToListAsync();
    }

    public async Task<bool> Update(Product Product)
    {
        var updatedResult = await _catalogContext
                                    .Products
                                    .ReplaceOneAsync(filter: o => o.Id == Product.Id, replacement: Product);

        return updatedResult.IsAcknowledged
            && updatedResult.ModifiedCount > 0;
    }
}
