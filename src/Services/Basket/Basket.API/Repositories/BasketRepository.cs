using Basket.API.Entities;
using Core.Caching;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ICache Cache;
        public BasketRepository(ICache cache) { 
            Cache = cache;
        }
        public async Task<ShoppingCart> GetAsync(string UserName)
        {
            if(Cache.IsKeyExisting(UserName))
            {
                
                var shoppingCart=await Cache.GetAsync<ShoppingCart>(UserName);
                shoppingCart.TotalPrice = shoppingCart.Items.Sum(i => i.Price);
                return shoppingCart;
            }
            ShoppingCart newShoppingCart = new ShoppingCart(UserName);

            return newShoppingCart;
        }
        public async Task<ShoppingCart> UpdateAsync(ShoppingCart cart)
        {
            await Cache.SetAsync<ShoppingCart>(cart.UserName, cart);
            return await Cache.GetAsync<ShoppingCart>(cart.UserName);
        }
        public async Task<bool> RemoveAsync(string UserName)
        {
            try
            {
                if (Cache.IsKeyExisting(UserName))
                {
                    await Cache.RemoveAsync(UserName);
                 
                }   
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
