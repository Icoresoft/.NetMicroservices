using AutoMapper;
using AutoMapper.Configuration.Conventions;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Features.Order.Commands.Checkout;
using Ordering.Application.Features.Order.Commands.UpdateOrder;
using Ordering.Application.Features.Order.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapper
{
    public  class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVM>().ReverseMap();
            CreateMap<Order, CheckoutCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
