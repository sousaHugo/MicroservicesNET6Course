using Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace Catalog.Infrastructure.Context;

internal class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration Configuration)
    {
        var client = new MongoClient(Configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(Configuration.GetValue<string>("DatabaseSettings:Database"));

        Products = database.GetCollection<Product>(Configuration.GetValue<string>("DatabaseSettings:CollectionName"));

        CatalogContextSeed.SeedData(Products);
    }
    public IMongoCollection<Product> Products { get; }
}
