using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        public BasketCheckoutConsumer(IMediator Mediator, IMapper Mapper, ILogger<BasketCheckoutConsumer> Logger)
        {
            _mediator = Mediator ?? throw new ArgumentNullException(nameof(Mediator));
            _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> Context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(Context.Message);

            var result = await _mediator.Send(command);

            _logger.LogInformation("BasketCheckoutEvent consumed successfully");
        }
    }
}
