using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Order.Commands.Checkout;

namespace Ordering.API.EventBus.Consumers
{
    public class BasketCheckoutEventConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutEventConsumer> _logger;
        public BasketCheckoutEventConsumer(IMediator mediator, IMapper mapper,ILogger<BasketCheckoutEventConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            _logger.LogInformation($"Message Received ...{context.Message.UserName} Total {context.Message.TotalPrice} ");
            var cmd=_mapper.Map<CheckoutCommand>(context.Message);
            var result=await _mediator.Send(cmd);
        }
    }
}
