using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShoppingController> _logger;
        public ShoppingController(IBasketService basketService, ICatalogService catalogService, IOrderService orderService, ILogger<ShoppingController> logger)
        {
            _basketService = basketService;
            _catalogService = catalogService;
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{UserName}")]
        [ProducesResponseType(typeof(ShoppingModel),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetAsync(string UserName)
        {
            var basket=await _basketService.GetBasket(UserName);

            //1:
            Parallel.ForEach(basket.Items, async (item,state,i) =>
            {
                var index = (int)i;
                var product = await _catalogService.GetCatalog(item.ProductId);

                //map additional field
                basket.Items[index].Description = product.Description;
                basket.Items[index].Summary = product.Summary;
                basket.Items[index].Category = product.Category;
                basket.Items[index].ImageFile = product.ImageFile;
            });

            //2:
            //foreach (var item in basket.Items)
            //{
            //    var product = await _catalogService.GetCatalog(item.ProductId);
            //    //map additional field
            //    item.Description = product.Description;
            //    item.Summary = product.Summary;
            //    item.Category = product.Category;
            //    item.ImageFile = product.ImageFile;
            //}

            var orders=await _orderService.GetOrdersByUserName(UserName);

            ShoppingModel shoppingModel = new()
            {
                UserName = UserName,
                Orders = orders,
                BasketWithProducts = basket
            };
            return Ok(shoppingModel);
        }
    }
}
