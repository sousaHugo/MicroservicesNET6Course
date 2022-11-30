using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient HttpClient)
        {
            _httpClient = HttpClient ?? throw new ArgumentNullException(nameof(HttpClient));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _httpClient.GetAsync("/api/v1/Product");

            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string Id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Product/{Id}");

            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCateory(string Category)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Product/GetByCategory/{Category}");

            return await response.ReadContentAs<List<CatalogModel>>();
        }
    }
}
