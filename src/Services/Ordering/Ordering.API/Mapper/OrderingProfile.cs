using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Order.Commands.Checkout;

namespace Ordering.API.Mapper
{
    public class OrderingProfile:Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutCommand,BasketCheckoutEvent> ().ReverseMap();
        }
    }
}
