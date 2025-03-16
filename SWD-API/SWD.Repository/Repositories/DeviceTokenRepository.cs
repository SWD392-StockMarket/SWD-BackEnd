using Microsoft.EntityFrameworkCore;
using SWD.Data;
using SWD.Data.Entities;
using SWD.Repository.Interface;

namespace SWD.Repository.Repositories;

public class DeviceTokenRepository : Repository<DeviceToken>, IDeviceTokenRepository
{
    private readonly StockMarketDbContext _dbcontext;
    public DeviceTokenRepository(StockMarketDbContext context) : base(context)
    {
        _dbcontext = context;
    }

    public async Task<bool> CreateDeviceToken(int userId, string fcmToken)
    {
        var deviceToken = new DeviceToken
        {
            UserId = userId,
            FCMToken = fcmToken,
        };

        await _dbcontext.AddAsync(deviceToken);
        return await _dbcontext.SaveChangesAsync() > 0;
    }

    public async Task<DeviceToken> GetDeviceToken()
    {
        var list = await _dbcontext.DeviceTokens.ToListAsync();
        return list[^1];
    }
}