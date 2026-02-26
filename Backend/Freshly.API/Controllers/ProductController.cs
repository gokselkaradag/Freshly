using System.Security.Claims;
using Freshly.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Freshly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetUserProducts()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var products = await _productService.GetUserProductsAsync(userId);
            
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var product = await _productService.GetProductByIdAsync(id, userId);
            
            if (product is null)
                return NotFound();
            
            return Ok(product);
        }
    }
}
