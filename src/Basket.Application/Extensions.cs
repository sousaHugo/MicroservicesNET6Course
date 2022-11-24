using Basket.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection Services)
    {
        Services.AddScoped<IBasketService, BasketService>();
        return Services;
    }
}
