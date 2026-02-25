using Freshly.DomainLayer.Entities;

namespace Freshly.DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}