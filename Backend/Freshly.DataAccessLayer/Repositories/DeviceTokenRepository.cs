using Freshly.DataAccessLayer.Context;
using Freshly.DataAccessLayer.Interfaces;
using Freshly.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freshly.DataAccessLayer.Repositories;

public class DeviceTokenRepository : IDeviceTokenRepository
{
    private readonly AppDbContext _context;
    
    public DeviceTokenRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<DeviceToken?> GetByTokenAsync(string token)
    {
        return await _context.DeviceTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<IEnumerable<DeviceToken>> GetByUserIdAsync(Guid userId)
    {
        return await _context.DeviceTokens.Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(DeviceToken deviceToken)
    {
        await _context.DeviceTokens.AddAsync(deviceToken);
    }

    public async Task UpdateAsync(DeviceToken deviceToken)
    {
        await Task.Run(() => _context.DeviceTokens.Update(deviceToken));
    }

    public async Task DeleteAsync(DeviceToken deviceToken)
    {
        await Task.Run(() => _context.DeviceTokens.Remove(deviceToken));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}