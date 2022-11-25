using Discount.Application.Services;
using Discount.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> _logger;
        private readonly IDiscountService _discountService;  

        public DiscountController(ILogger<DiscountController> Logger, IDiscountService DiscountService)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _discountService = DiscountService ?? throw new ArgumentNullException(nameof(DiscountService));
        }

        [HttpGet("{ProductName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetCouponByProductNameAsync(string ProductName)
        {
            return Ok(await _discountService.GetCouponByProductNameAsync(ProductName));
        }
        [HttpPost(Name = "CreateDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Coupon>> CreateDiscountAsync([FromBody] Coupon Coupon)
        {
            await _discountService.CreateCouponAsync(Coupon);

            return CreatedAtAction(nameof(GetCouponByProductNameAsync), new { ProductName = Coupon.ProductName }, Coupon);
        }

        [HttpPut("{Id}", Name = "UpdateDiscount")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Coupon>> UpdateDiscountAsync(int Id, [FromBody] Coupon Coupon)
        {
            if (Id != Coupon.Id)
                return BadRequest($"This Id {Id} doesn't belong to the Coupon");

            await _discountService.UpdateCouponAsync(Coupon);

            return NoContent();
        }

        [HttpDelete("{ProductName}", Name = "DeleteDiscount")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<Coupon>> DeleteDiscountAsync(string ProductName)
        {
            await _discountService.DeleteCouponAsync(ProductName);

            return NoContent();
        }
    }
}