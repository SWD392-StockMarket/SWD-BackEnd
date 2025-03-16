using SWD.Data.Entities;

namespace SWD.Service.Interface;

public interface IDeviceTokenService
{
    Task<bool> CreateDeviceToken(int userId, string fcmToken);
    
    Task<DeviceToken> GetFirstToken();
}