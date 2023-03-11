using AspnetRunBasics.Models;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
        Task<BasketModel> UpdateBasket(string UserName,BasketItemModel model);
        Task CheckoutBasket(BasketCheckoutModel model);
    }
}