using Basket.Domain.Entities;

namespace Basket.Application.Services;

public interface IBasketService
{
    Task<ShoppingCart> GetBasketByUsername(string Username);
    Task<ShoppingCart> SaveUsernameBasket(ShoppingCart Basket);
    Task ClearBasket(string Username);
}
