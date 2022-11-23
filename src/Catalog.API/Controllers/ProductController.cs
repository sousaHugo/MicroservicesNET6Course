using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> Logger, IProductService ProductService)
        {
            _logger = Logger;
            _productService = ProductService;
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
        {
            return Ok(await _productService.GetAllAsync());
        }
        [HttpGet("{Id}", Name = "GetById")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetByIdAsync(string Id)
        {
            try
            {
                return Ok(await _productService.GetByIdAsync(Id));
            }
            catch(Exception)
            {
                _logger.LogError($"Product with id: {Id} not found.");
                return NotFound();
            }
        }
        [HttpGet("{Category}", Name = "GetByCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryAsync(string Category)
        {
            return Ok(await _productService.GetByCategoryAsync(Category));
        }
        [HttpGet("{Name}", Name = "GetByName")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByNameAsync(string Name)
        {
            return Ok(await _productService.GetByNameAsync(Name));
        }
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateAsync([FromBody] Product Product)
        {
            await _productService.Create(Product);

            return CreatedAtAction(nameof(GetByIdAsync), new { Id = Product.Id }, Product);
        }
        [HttpPut("{Id}", Name = "Update")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync(string Id, [FromBody] Product Product)
        {
            if (!Id.Equals(Product.Id))
                return BadRequest($"Product to Update doesn't match with the id: {Id}");
                
           if(await _productService.Update(Product))
                return NoContent();

            return Problem($"An unexpected error occurred updating the product with id: {Id}");
        }
        [HttpDelete("{Id}", Name = "Delete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            if(await _productService.Delete(Id))
                return NoContent();

            return Problem($"An unexpected error occurred deleting the product with id: {Id}");
        }
    }
}