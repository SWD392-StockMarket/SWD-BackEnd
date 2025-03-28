using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD.Data;
using SWD.Data.DTOs.UsersStats;
using SWD.Service.Interface;

namespace SWD.Service.Services;

public class UsersStatsService: IUsersStatsService
{
    private readonly StockMarketDbContext _context;

    public UsersStatsService(StockMarketDbContext context)
    {
        _context = context;
    }

    public async Task<UsersStatsDTO> GetUsersStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var newUsers = await _context.Users
            .Where(u => u.CreatedAt >= DateTime.Now.AddDays(-30))
            .CountAsync();
        
        
        
        var subscriptionUsers = await _context.UserRoles
            .Where(ur => ur.RoleId == 2)
            .Select(ur => ur.UserId)
            .Distinct()
            .CountAsync();
        
        var nonSubscriptionUsers = totalUsers - subscriptionUsers;
        
        var totalUsersWithRoles = await _context.UserRoles
            .Where(ur => ur.RoleId == 2 || ur.RoleId == 4) // Filter for RoleId 2 or 4
            .Select(ur => ur.UserId)
            .Distinct() // Ensure no duplicate users
            .CountAsync();
        var churnRate = totalUsersWithRoles == 0
            ? 0
            : (double)subscriptionUsers / totalUsersWithRoles * 100;
        
        // Calculate GrowthData (cumulative users by month)
        var growthData = await _context.Users
            .Where(u => u.CreatedAt.HasValue) // Filter out null CreatedAt
            .GroupBy(u => new { u.CreatedAt.Value.Year, u.CreatedAt.Value.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new
            {
                Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"), // e.g., "Jan"
                Count = g.Count()
            })
            .ToListAsync();

        // Build cumulative GrowthData
        var cumulativeGrowthData = new List<UserGrowthEntry>();
        int cumulativeCount = 0;
        foreach (var entry in growthData)
        {
            cumulativeCount += entry.Count;
            cumulativeGrowthData.Add(new UserGrowthEntry
            {
                Period = entry.Period,
                UserCount = cumulativeCount
            });
        }
        
        return new UsersStatsDTO
        {
            TotalUsers = totalUsers,
            NewUsers = newUsers,
            SubscriptionUsers = subscriptionUsers,
            NonSubscriptionUsers = nonSubscriptionUsers,
            ChurnRate = $"{churnRate:0.##}%",
            GrowthData = cumulativeGrowthData
        };
    }
}