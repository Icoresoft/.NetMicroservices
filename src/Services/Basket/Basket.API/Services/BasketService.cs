using Basket.API.Entities;
using Basket.API.Repositories;

namespace Basket.API.Services
{
    public class BasketService
    {
        private readonly IBasketRepository basketRepository;
        public BasketService(IBasketRepository repo)
        {
            this.basketRepository = repo;
        }
        public async Task<ShoppingCart> GetAsync(string UserName)
        {
            return await basketRepository.GetAsync(UserName);
        }

        public async Task<ShoppingCart> AddItemAsync(string UserName,ShoppingCartItem item)
        {
            var shoppingCard= await basketRepository.GetAsync(UserName);

            shoppingCard.Items.Add(item);

            return await basketRepository.UpdateAsync(shoppingCard);
        }

        public async Task  RemoveAsync(string UserName)
        {
            await basketRepository.RemoveAsync(UserName);
        }
    }
}
