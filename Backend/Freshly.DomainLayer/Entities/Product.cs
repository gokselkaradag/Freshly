using Freshly.DomainLayer.Enums;

namespace Freshly.DomainLayer.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImagePath { get; set; }  = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.PendingOCR;
    public User User { get; set; } = null!;
    public bool Notified { get; set; }  = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}