using Freshly.BusinessLayer.Interfaces;
using Freshly.BusinessLayer.Models;
using Freshly.DataAccessLayer.Interfaces;
using Freshly.DomainLayer.Entities;
using Freshly.DomainLayer.Enums;
using Freshly.DomainLayer.Messages;

namespace Freshly.BusinessLayer.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMessagePublisher _messagePublisher;

    public ProductService(IProductRepository productRepository, IMessagePublisher messagePublisher)
    {
        _productRepository = productRepository;
        _messagePublisher = messagePublisher;
    }


    public async Task<IEnumerable<ProductResult>> GetUserProductsAsync(Guid userId)
    {
        var products = await _productRepository.GetByUserIdAsync(userId);
        return products.Select(p => new ProductResult(
            p.Id,
            p.Name,
            p.ImagePath,
            p.ExpirationDate,
            p.Status.ToString(),
            p.CreatedAt
        ));
    }

    public async Task<ProductResult?> GetProductByIdAsync(Guid productId, Guid userId)
    {
        //Ürünü getir.
        var product = await _productRepository.GetByIdAsync(productId);
        
        //Ürün yoksa veya bu kullanıcıya ait değilse null dönüyoruz.
        if(product is null || product.UserId != userId)
            return null;
        
        //Ürün var ise ProductResult'a dönüştürüp dönüyoruz. 
        return new ProductResult(
            product.Id,
            product.Name,
            product.ImagePath,
            product.ExpirationDate,
            product.Status.ToString(),
            product.CreatedAt
        );
    }

    public async Task<ProductUploadResult> UploadProductAsync(Guid userId, string name, string imagePath)
    {
        //Ürün oluşturma.
        var product = new Product
        {
            UserId = userId,
            Name = name,
            ImagePath = imagePath
        };
        
        //Veritabanına kayıt ediyoruz.
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        
        //RabbitMQ'ya mesajı gönderiyoruz.
        await _messagePublisher.PublishOcrJobAsync(new OcrJobMessage
        {
            ProductId = product.Id,
            ImagePath = product.ImagePath
        });
        
        //Sonucu döndürüp mesajı kullanıcıya iletiyoruz.
        return new ProductUploadResult(product.Id, product.Status.ToString(), "Ürün yüklendi, SKT okunuyor...");
    }

    public async Task SetManualExpirationAsync(Guid productId, DateTime expirationDate, Guid userId)
    {
        //Ürünü getir.
        var product = await _productRepository.GetByIdAsync(productId);

        //Ürün yoksa veya bu kullanıcıya ait değilse dur.
        if (product is null || product.UserId != userId)
            return;
        
        //Ürün var ve bu kullanıcıya ait ise =>
        product.ExpirationDate = expirationDate;
        product.Status = ProductStatus.OCRCompleted;
        
        //Veritabanına kayıt ediyoruz.
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid productId, Guid userId)
    {
        //Ürünü getir.
        var product = await _productRepository.GetByIdAsync(productId);
        
        //Ürün yoksa veya bu kullanıcıya ait değilse dur.
        if (product is null || product.UserId != userId)
            return;
        
        //Veritabanına kayıt ediyoruz.
        await _productRepository.DeleteAsync(product);
        await _productRepository.SaveChangesAsync();
    }
}