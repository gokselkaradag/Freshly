namespace Freshly.BusinessLayer.Interfaces;

public interface ITokenService
{
    string GenerateToken(Guid userId, string email);
}