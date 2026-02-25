using Freshly.BusinessLayer.Models;

namespace Freshly.BusinessLayer.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResult>> GetUserProductsAsync(Guid userId);
    Task<ProductResult?> GetProductByIdAsync(Guid productId, Guid userId);
    Task<ProductUploadResult> UploadProductAsync(Guid userId, string name, string imagePath);
    Task SetManualExpirationAsync(Guid productId, DateTime expirationDate,Guid userId);
    Task DeleteProductAsync(Guid productId, Guid userId);
}