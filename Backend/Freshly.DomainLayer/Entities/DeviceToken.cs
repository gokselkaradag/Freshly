namespace Freshly.DomainLayer.Entities;

public class DeviceToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Platform { get; set; }  = string.Empty;
    public User User { get; set; } = null!;
}