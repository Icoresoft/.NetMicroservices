using Basket.API.Entities;
using System.Data;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetAsync(string UserName);
        Task<ShoppingCart> UpdateAsync(ShoppingCart cart);
        Task<bool> RemoveAsync(string UserName);

    }
}
