using Freshly.BusinessLayer.Interfaces;
using Freshly.BusinessLayer.Models;
using Freshly.DataAccessLayer.Interfaces;
using Freshly.DomainLayer.Entities;

namespace Freshly.BusinessLayer.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository  _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }


    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        //Email kayıtlı mı değil mi kontrol ediyoruz.
        if (await _userRepository.ExistsAsync(email))
            return new AuthResult(false, null, null, null, "Email already in use");

        //Kullanıcı oluşturuyoruz.
        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        
        //Kullanıcıyı veritabanına kayıt ediyoruz.
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        //Kullanıcıya özel token üretiyoruz.
        var token = _tokenService.GenerateToken(user.Id,  user.Email);
        return new AuthResult(true, token, user.Email, user.Id,null);
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        //Kullanıcıyı buluruz.
        var user = await _userRepository.GetByEmailAsync(email);
        
        //Kullanıcı veritabanında mevcut mu ? Mevcut ise şifre kontrolü yapıyoruz.
        if(user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new AuthResult(false, null, null, null, "Invalid credentials");
        
        //Giriş yapan kullanıcıya özel token üretiyoruz.
        var token = _tokenService.GenerateToken(user.Id,  user.Email);
        return new AuthResult(true, token, user.Email, user.Id,null);
    }
}