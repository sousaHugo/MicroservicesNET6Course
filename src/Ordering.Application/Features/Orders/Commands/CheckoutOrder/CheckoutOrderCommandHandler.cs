using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    public CheckoutOrderCommandHandler(IOrderRepository OrderRepository, IMapper Mapper, IEmailService EmailService, ILogger<CheckoutOrderCommandHandler> Logger)
    {
        _orderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        _emailService = EmailService ?? throw new ArgumentNullException(nameof(EmailService));
        _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
    }

    public async Task<int> Handle(CheckoutOrderCommand Request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(Request);

        var newOrder = await _orderRepository.AddAsync(orderEntity);

        _logger.LogInformation($"Order {newOrder.Id} is successfully created.");

        await SendEmail(newOrder);

        return newOrder.Id;
    }

    private async Task SendEmail(Order Order)
    {
        var email = new Email() { To = "sousa.miguel.hugo@gmail.com", Body = $"Order was created", Subject = $"Order {Order.Id} is successfully created." };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Order {Order.Id} failed due to an error with the email service: {ex.Message}");
        }
    }
}
