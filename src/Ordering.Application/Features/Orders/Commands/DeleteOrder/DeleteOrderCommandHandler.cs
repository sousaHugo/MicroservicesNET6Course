using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository OrderRepository, IMapper Mapper, ILogger<DeleteOrderCommandHandler> Logger)
    {
        _orderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
        _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
    }
    public async Task<Unit> Handle(DeleteOrderCommand Request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(Request.Id);

        if(orderToDelete is null)
            throw new NotFoundException(nameof(Order), Request.Id);

        await _orderRepository.DeleteAsync(orderToDelete);

        _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

        return Unit.Value;
    }
}
