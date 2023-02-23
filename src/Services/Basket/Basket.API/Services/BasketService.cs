using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services.Grpc;

namespace Basket.API.Services
{
    public class BasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly GrpcHealthCheckService _grpcHealthCheckService;
        private readonly GrpcDiscountService _grpcDiscountService;

        public BasketService(IBasketRepository basketRepository, GrpcHealthCheckService grpcHealthCheckService, GrpcDiscountService grpcDiscountService)
        {
            _basketRepository = basketRepository;
            _grpcHealthCheckService = grpcHealthCheckService;
            _grpcDiscountService = grpcDiscountService;
        }

        public async Task<string> Ping(string Message)
        {
            var reply= await _grpcHealthCheckService.Ping(Message);

            return reply.ServerDateTime;
        } 
        public async Task<ShoppingCart> GetAsync(string UserName)
        {
            return await _basketRepository.GetAsync(UserName);
        }

        public async Task<ShoppingCart> AddItemAsync(string UserName,ShoppingCartItem item)
        {
            var shoppingCard= await _basketRepository.GetAsync(UserName);

            var coupon = await _grpcDiscountService.GetProductDiscountAsync(item.ProductCode);
            item.Price -=Convert.ToDecimal(coupon.Amount);
            shoppingCard.Items.Add(item);

            return await _basketRepository.UpdateAsync(shoppingCard);
        }

        public async Task  RemoveAsync(string UserName)
        {
            await _basketRepository.RemoveAsync(UserName);
        }
    }
}
