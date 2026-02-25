using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Freshly.BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Freshly.BusinessLayer.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }


    public string GenerateToken(Guid userId, string email)
    {
        //Secret key oluşturuyoruz.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        
        //Claim tanımlamaları, içlerine tokenler gömülecek.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        //Token oluşturma.
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        
        //Token string olarak döndür.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}