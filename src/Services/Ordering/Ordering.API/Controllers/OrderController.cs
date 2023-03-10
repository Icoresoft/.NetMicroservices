using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Order.Commands.Checkout;
using Ordering.Application.Features.Order.Commands.DeleteOrder;
using Ordering.Application.Features.Order.Commands.UpdateOrder;
using Ordering.Application.Features.Order.Queries.GetOrdersList;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public OrderController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }
        [HttpGet("{UserName}")]
        public async Task<ActionResult<List<OrdersVM>>> GetOrdersList(string UserName)
        {
            GetOrdersListQuery Query = new GetOrdersListQuery(UserName);
            var result=await _mediatR.Send(Query);
            return Ok(result);
        }
        [HttpPost("Checkout")]
        public async Task<ActionResult<Int32>> Checkout([FromBody] CheckoutCommand checkoutCommand)
        {
            var result=await _mediatR.Send(checkoutCommand);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            await _mediatR.Send(updateOrderCommand);
            return Ok();
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(Int32 OrderId)
        {
            var deleteOrderCommand=new DeleteOrderCommand(OrderId);
            await _mediatR.Send(deleteOrderCommand);
            return Ok();
        }
    }
}
