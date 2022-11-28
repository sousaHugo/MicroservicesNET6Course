
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection Services)
    {
        Services.AddScoped<ICatalogContext, CatalogContext>();
        Services.AddScoped<IProductRepository, ProductRepository>();

        return Services;
    }
}
