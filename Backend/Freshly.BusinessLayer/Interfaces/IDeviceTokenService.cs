namespace Freshly.BusinessLayer.Interfaces;

public interface IDeviceTokenService
{
    Task RegisterDeviceTokenAsync(Guid userId, string token, string platform);
    Task RemoveTokenAsync(Guid userId, string token);
}