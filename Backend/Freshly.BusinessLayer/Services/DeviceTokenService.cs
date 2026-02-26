using Freshly.BusinessLayer.Interfaces;
using Freshly.DataAccessLayer.Interfaces;
using Freshly.DomainLayer.Entities;

namespace Freshly.BusinessLayer.Services;

public class DeviceTokenService : IDeviceTokenService
{
    private readonly IDeviceTokenRepository _deviceTokenRepository;

    public DeviceTokenService(IDeviceTokenRepository deviceTokenRepository)
    {
        _deviceTokenRepository = deviceTokenRepository;
    }


    public async Task RegisterDeviceTokenAsync(Guid userId, string token, string platform)
    {
        //Veritabanında bu token var mı kontrol ediyoruz.
        var existingToken = await _deviceTokenRepository.GetByTokenAsync(token);

        if (existingToken is not null)
        {
            //token var ise güncelle
            existingToken.UserId = userId;
            existingToken.Platform = platform;
            await _deviceTokenRepository.UpdateAsync(existingToken);
        }
        else
        {
            //token yok ise yeni token ekle 
            await _deviceTokenRepository.AddAsync(new DeviceToken
            {
                UserId = userId,
                Platform = platform,
                Token = token
            });
        }
        
        //Yeni tokenı kayıt et.
        await _deviceTokenRepository.SaveChangesAsync();
    }

    public async Task RemoveTokenAsync(Guid userId, string token)
    {
        //Veritabanında bu token var mı kontrol ediyoruz.
        var existingToken = await _deviceTokenRepository.GetByTokenAsync(token);
        
        //Token yok ise veya kullanıcıya ait değilse dur.
        if (existingToken is not null || existingToken.UserId != userId)
            return;
        
        //Veritabanına kayıt ediyoruz.
        await _deviceTokenRepository.DeleteAsync(existingToken);
        await _deviceTokenRepository.SaveChangesAsync();
    }
}