using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IOrderRepository OrderRepository, IMapper Mapper, ILogger<UpdateOrderCommandHandler> Logger)
    {
        _orderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
    }

    public async Task<Unit> Handle(UpdateOrderCommand Request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(Request.Id);
        if (orderToUpdate == null)
            throw new NotFoundException(nameof(Order), Request.Id);

        _mapper.Map(Request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(orderToUpdate);

        _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated");

        return Unit.Value;
    }
}
