using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System.Linq.Expressions;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;
        public OrderController(ILogger<OrderController> Logger, IMediator Mediator)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _mediator = Mediator ?? throw new ArgumentNullException(nameof(Mediator));
        }

        [HttpGet("{Username}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUsernameAsync(string Username)
        {
            var query = new GetOrderListQuery(Username);

            return Ok(await _mediator.Send(query));
        }
        [HttpPost("CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrderAsync([FromBody]CheckoutOrderCommand Command)
        {
            return Ok(await _mediator.Send(Command));
        }
        [HttpPut("{Id}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdateOrderAsync(int Id, [FromBody] UpdateOrderCommand Command)
        {
            await _mediator.Send(Command);

            return NoContent();
        }
        [HttpDelete("{Id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> DeleteOrderAsync(int Id)
        {
            var command = new DeleteOrderCommand() { Id = Id };
            
            await _mediator.Send(command);

            return NoContent();
        }
    }
}