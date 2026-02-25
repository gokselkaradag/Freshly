using Freshly.DomainLayer.Entities;

namespace Freshly.DataAccessLayer.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Product>> GetExpiringProductsAsync(int days);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task SaveChangesAsync();
}