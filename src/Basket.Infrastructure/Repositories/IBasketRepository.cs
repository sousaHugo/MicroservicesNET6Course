using Basket.Domain.Entities;
namespace Basket.Infrastructure.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string Username);
    Task<ShoppingCart> UpdateBasket(ShoppingCart Basket);
    Task DeleteBasket(string Username);
}
