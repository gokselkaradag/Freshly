using Freshly.BusinessLayer.Models;

namespace Freshly.BusinessLayer.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string password);
    Task<AuthResult> LoginAsync(string email, string password);
}