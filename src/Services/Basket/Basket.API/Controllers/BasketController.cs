using Basket.API.Entities;
using Basket.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketService basketService;
        public BasketController(BasketService basketService)
        {
            this.basketService = basketService;
        }
        [HttpGet]
        public async Task<ShoppingCart> GetAsync(string UserName)
        {
            return await basketService.GetAsync(UserName);
        }

        [HttpPut]
        public async Task<ShoppingCart> AddItemAsync(string UserName,ShoppingCartItem item)
        {
            return await basketService.AddItemAsync(UserName,item);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(string UserName)
        {
            try
            {
                await basketService.RemoveAsync(UserName);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
     
        }
    }
}
