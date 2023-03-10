using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _Client;

        public CatalogService(HttpClient client)
        {
            _Client = client;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response =await _Client.GetAsync("/api/catalog");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string Id)
        {
            var response = await _Client.GetAsync($"/api/catalog/{Id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string Category)
        {
            var response = await _Client.GetAsync($"/api/catalog/GetByCategory/{Category}");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }
    }
}
