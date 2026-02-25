using Freshly.DomainLayer.Entities;

namespace Freshly.DataAccessLayer.Interfaces;

public interface IDeviceTokenRepository
{
    Task<DeviceToken?> GetByTokenAsync(string token);
    Task<IEnumerable<DeviceToken>> GetByUserIdAsync(Guid userId);
    Task AddAsync(DeviceToken deviceToken);
    Task UpdateAsync(DeviceToken deviceToken);
    Task DeleteAsync(DeviceToken deviceToken);
    Task SaveChangesAsync();
}