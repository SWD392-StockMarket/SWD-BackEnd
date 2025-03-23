using System.Threading.Tasks;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services;

public class DeviceTokenService : IDeviceTokenService
{
    private readonly IDeviceTokenRepository _deviceTokenRepository;

    public DeviceTokenService(IDeviceTokenRepository deviceTokenRepository)
    {
        _deviceTokenRepository = deviceTokenRepository;
    }
    
    public async Task<bool> CreateDeviceToken(int userId, string fcmToken)
    {
        return await _deviceTokenRepository.CreateDeviceToken(userId, fcmToken);
    }

    public async Task<DeviceToken> GetFirstToken()
    {
        return await _deviceTokenRepository.GetDeviceToken();
    }
}