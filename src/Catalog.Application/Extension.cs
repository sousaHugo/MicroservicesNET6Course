using Catalog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection Services)
    {
        Services.AddScoped<IProductService, ProductService>();

        return Services;
    }
}
