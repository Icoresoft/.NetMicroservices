using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _Client;

        public OrderService(HttpClient client)
        {
            _Client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string UserName)
        {
            var response = await _Client.GetAsync($"/api/order/{UserName}");
            return await response.ReadContentAs<IEnumerable<OrderResponseModel>>();
        }
    }
}
