using Basket.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    public BasketRepository(IDistributedCache RedisCache)
    {
        _redisCache = RedisCache ?? throw new ArgumentNullException(nameof(RedisCache));
    }
    public async Task DeleteBasket(string Username)
    {
        await _redisCache.RemoveAsync(Username);
    }

    public async Task<ShoppingCart> GetBasket(string Username)
    {
        var basket = await _redisCache.GetStringAsync(Username);

        if (string.IsNullOrEmpty(basket))
            return null;

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart Basket)
    {
        await _redisCache.SetStringAsync(Basket.Username, JsonConvert.SerializeObject(Basket));

        return await GetBasket(Basket.Username);
    }
}
