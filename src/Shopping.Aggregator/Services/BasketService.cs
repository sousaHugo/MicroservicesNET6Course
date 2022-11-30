using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient HttpClient)
        {
            _httpClient = HttpClient ?? throw new ArgumentNullException(nameof(HttpClient));
        }
        public async Task<BasketModel> GetBasket(string Username)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Basket/{Username}");

            return await response.ReadContentAs<BasketModel>();
        }
    }
}
