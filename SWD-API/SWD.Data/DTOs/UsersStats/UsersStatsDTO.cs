namespace SWD.Data.DTOs.UsersStats;

public class UsersStatsDTO
{
    public int TotalUsers { get; set; }
    public int NewUsers { get; set; }
    public int SubscriptionUsers { get; set; }
    public int NonSubscriptionUsers { get; set; }
    public string ChurnRate { get; set; }
    public List<UserGrowthEntry> GrowthData { get; set; } 
}

public class UserGrowthEntry
{
    public string Period { get; set; }
    public int UserCount { get; set; }
}