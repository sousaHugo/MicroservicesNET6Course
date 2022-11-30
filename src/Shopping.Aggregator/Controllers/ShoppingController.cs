using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ILogger<ShoppingController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ILogger<ShoppingController> Logger, ICatalogService CatalogService, IBasketService BasketService,
            IOrderService OrderService)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _catalogService = CatalogService ?? throw new ArgumentNullException(nameof(CatalogService));
            _basketService = BasketService ?? throw new ArgumentNullException(nameof(BasketService));
            _orderService = OrderService ?? throw new ArgumentNullException(nameof(OrderService));

        }

        [HttpGet("{Username}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShoppingAsync(string Username)
        {
            var basket = await _basketService.GetBasket(Username);

            foreach(var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrderByUsername(Username);

            var shoppingModel = new ShoppingModel()
            {
                UserName = Username,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}