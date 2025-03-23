using System.Threading.Tasks;
using SWD.Data.Entities;

namespace SWD.Service.Interface;

public interface IPythonExecuteService
{
    Task GetStockHistory(string symbol, string startDate, string endDate);
}