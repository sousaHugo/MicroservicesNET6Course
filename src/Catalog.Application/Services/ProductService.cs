using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    public ProductService(IProductRepository ProductRepository, ILogger<ProductService> Logger) 
    {
        _logger = Logger;
        _productRepository = ProductRepository; 
    }

    public async Task Create(Product Product)
    {
        var product = await _productRepository.GetByNameAsync(Product.Name);

        if (product.Any())
            throw new Exception($"The Product {Product.Name} already exists.");

        await _productRepository.Create(Product);
    }

    public async Task<bool> Delete(string Id)
    {
        if(!await Exists(Id))
            throw new Exception($"Product {Id} doesn't exists!");

        return await _productRepository.Delete(Id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string Category)
    {
        return await _productRepository.GetByCategoryAsync(Category);
    }

    public async Task<Product> GetByIdAsync(string Id)
    {
        var product = await _productRepository.GetByIdAsync(Id);

        if (product is null)
            throw new Exception($"Product {Id} doesn't exists!");

        return product;
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string Name)
    {
        return await _productRepository.GetByNameAsync(Name);
    }

    public async Task<bool> Update(Product Product)
    {
        if (!await Exists(Product.Id))
            throw new Exception($"Product {Product.Id} doesn't exists!");

        return await _productRepository.Update(Product);
    }

    public async Task<bool> Exists(string Id)
    {
        var product = await _productRepository.GetByIdAsync(Id);

        return product is null ? false : true;
    }
}
