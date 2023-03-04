using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Commands.Checkout
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutCommandHandler> _logger;

        public CheckoutCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService mailService, ILogger<CheckoutCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = mailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Domain.Entities.Order>(request);
            await _orderRepository.AddAsync(order);
            await _orderRepository.Commit();
            _logger.LogInformation($"order created. number # {order.Id}");
            await SendEmail(order);
            return order.Id;
        }
        public async Task<bool>  SendEmail(Domain.Entities.Order order)
        {
            try
            {
                Email mail = new Email { To = order.EmailAddress, Subject = "", Body = "" };
                return await _emailService.SendEmailAsync(mail);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
