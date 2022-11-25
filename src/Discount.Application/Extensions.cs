using Discount.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection Services)
    {
        Services.AddScoped<IDiscountService, DiscountService>();
        return Services;
    }
}
