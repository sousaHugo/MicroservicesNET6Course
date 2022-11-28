using Basket.Domain.Entities;
using Basket.Domain.Interfaces;

namespace Basket.Application.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _repository;
    public BasketService(IBasketRepository Repository)
    {
        _repository = Repository ?? throw new ArgumentNullException(nameof(Repository));
    }
    public async Task ClearBasket(string Username)
    {
        await _repository.DeleteBasket(Username);
    }

    public async Task<ShoppingCart> GetBasketByUsername(string Username)
    {
        return await _repository.GetBasket(Username) ??
            new ShoppingCart(Username);
    }

    public async Task<ShoppingCart> SaveUsernameBasket(ShoppingCart Basket)
    {
        return await _repository.UpdateBasket(Basket);
    }
}
