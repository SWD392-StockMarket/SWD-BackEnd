using SWD.Data.Entities;

namespace SWD.Repository.Interface;

public interface IDeviceTokenRepository : IRepository<DeviceToken>
{
    Task<bool> CreateDeviceToken(int userId, string fcmToken);
    
    Task<DeviceToken> GetDeviceToken();
}