namespace Freshly.DomainLayer.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; }  = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //Neden ICollection çünkü, EF Core bu propertyleri görünce otomatik ilişki kuruyor.
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<DeviceToken> DeviceTokens { get; set; } = new List<DeviceToken>();
}