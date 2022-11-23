using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Context;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
}
