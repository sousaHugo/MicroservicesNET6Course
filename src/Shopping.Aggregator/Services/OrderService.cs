using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient HttpClient)
        {
            _httpClient = HttpClient ?? throw new ArgumentNullException(nameof(HttpClient));
        }
        public async Task<IEnumerable<OrderResponseModel>> GetOrderByUsername(string Username)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Order/{Username}");

            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
