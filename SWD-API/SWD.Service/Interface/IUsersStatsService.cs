using System.Threading.Tasks;
using SWD.Data.DTOs.UsersStats;

namespace SWD.Service.Interface;

public interface IUsersStatsService
{
    Task<UsersStatsDTO> GetUsersStatsAsync();
}