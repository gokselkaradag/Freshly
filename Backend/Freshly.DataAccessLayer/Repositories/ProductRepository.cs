using Freshly.DataAccessLayer.Context;
using Freshly.DataAccessLayer.Interfaces;
using Freshly.DomainLayer.Entities;
using Freshly.DomainLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace Freshly.DataAccessLayer.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Products.Where(p => p.UserId == userId).ToListAsync();
    }
    
    //Bu method NotificationWorker tarafından her 24 saatte bir çağrılır.
    //3 gün içinde sona erecek, OCR tamamlanmış ve henüz bildirim gönderilmemiş ürünleri getirir.
    public async Task<IEnumerable<Product>> GetExpiringProductsAsync(int days)
    {
        return await _context.Products
            .Where(p => p.ExpirationDate <= DateTime.UtcNow.AddDays(days)
                && p.Notified == false
                && p.Status == ProductStatus.OCRCompleted)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        await Task.Run(() => _context.Products.Update(product));
    }

    public async Task DeleteAsync(Product product)
    {
        await Task.Run(() => _context.Products.Remove(product));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}