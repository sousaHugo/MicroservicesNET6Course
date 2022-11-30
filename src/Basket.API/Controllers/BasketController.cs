using AutoMapper;
using Basket.API.GrpcServices;
using Basket.Application.Services;
using Basket.Domain.Entities;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketService _basketService;
        private readonly DiscountGrpcService _discountService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(ILogger<BasketController> Logger, IBasketService BasketService, DiscountGrpcService DiscountService,
            IMapper Mapper, IPublishEndpoint PublishEndpoint)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _basketService = BasketService ?? throw new ArgumentNullException(nameof(BasketService));
            _discountService = DiscountService ?? throw new ArgumentNullException(nameof(DiscountService));
            _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _publishEndpoint = PublishEndpoint ?? throw new ArgumentNullException(nameof(PublishEndpoint));
        }

        [HttpGet("{Username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string Username)
        {
            return Ok(await _basketService.GetBasketByUsername(Username));
        }

        [HttpPut("{Username}", Name = "UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasketAsync(string Username, [FromBody]ShoppingCart Basket)
        {
            if (!Username.Equals(Basket.Username))
                return BadRequest($"The Basket to Update doesn't belongs to the User {Username}");

            foreach(var item in Basket.Items)
            {
                var discount = await _discountService.GetDiscountByProductName(item.ProductName);
                item.Price -= discount.Amount;
            }

            return CreatedAtAction(nameof(GetBasketAsync), new { Username = Username }, await _basketService.SaveUsernameBasket(Basket));
        }

        [HttpDelete("{Username}", Name = "ClearBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ClearBasketAsync(string Username)
        {
            await _basketService.ClearBasket(Username);
            
            return NoContent();
        }

        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckout BasketCheckout)
        {
            var basket = await _basketService.GetBasketByUsername(BasketCheckout.UserName);
            if (basket is null)
                return BadRequest();


            var eventMessage = _mapper.Map<BasketCheckoutEvent>(BasketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);

            await _basketService.ClearBasket(BasketCheckout.UserName);

            return Accepted();
        }
    }
}