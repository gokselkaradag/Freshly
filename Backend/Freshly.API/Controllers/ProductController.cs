using System.Security.Claims;
using Freshly.API.Dtos;
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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadProduct([FromForm] UploadProductRequest request, IFormFile image)
        {
            //Kayıt klasörü oluşturuyoruz.
            var uploadsFolder = Path.Combine("wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder);
            
            //Dosya adı oluşturuyoruz.
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            
            //Dosyayı kayıt ediyoruz.
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _productService.UploadProductAsync(userId, request.Name, filePath);
            
            return Ok(result);
        }

        [HttpPatch("{id}/expiration")]
        public async Task<IActionResult> SetManuelExpiration(Guid id, [FromBody] SetExpirationRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await _productService.SetManualExpirationAsync(id, request.ExpiraitonDate, userId);
            
            return Ok(new MessageResponse("SKT Güncellendi."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await _productService.DeleteProductAsync(id, userId);
            
            return Ok(new MessageResponse("Ürün Silindi"));
        }
    }
}