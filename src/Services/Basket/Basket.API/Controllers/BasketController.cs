using AutoMapper;
using Basket.API.Entities;
using Basket.API.Services;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketService basketService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(BasketService basketService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this.basketService = basketService;
            this._mapper = mapper;
            this._publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public async Task<ShoppingCart> GetAsync(string UserName)
        {
            return await basketService.GetAsync(UserName);
        }
        [HttpGet("ping")]
        public async Task<string> Ping(string Message)
        {
            return await basketService.Ping(Message);
        }
        [HttpPut]
        public async Task<ShoppingCart> AddItemAsync(string UserName, ShoppingCartItem item)
        {
            return await basketService.AddItemAsync(UserName, item);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(string UserName)
        {
            try
            {
                await basketService.RemoveAsync(UserName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout bascketCheckout)
        {
            //1: get user basket
            var basket = await basketService.GetAsync(bascketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            //2: set order total price 
            var basketCheckoutEvent = _mapper.Map<BasketCheckoutEvent>(bascketCheckout);
            basketCheckoutEvent.TotalPrice = basket.TotalPrice;

            //3: Fire basketCheckoutEvent Event
            await _publishEndpoint.Publish(basketCheckoutEvent);

            //4: delete use basket 
            await basketService.RemoveAsync(bascketCheckout.UserName);
            return Accepted();
        }
    }
}
