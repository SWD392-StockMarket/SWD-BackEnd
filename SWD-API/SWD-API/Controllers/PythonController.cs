using Microsoft.AspNetCore.Mvc;
using SWD.Service.Interface;

namespace SWD_API.Controllers;

[Route("api/v{version:apiVersion}/python")]
[ApiVersion("1.0")]
[ApiController]
public class PythonController : ControllerBase
{
    private readonly IPythonExecuteService _pythonExecuteService;

    public PythonController(IPythonExecuteService pythonExecuteService)
    {
        _pythonExecuteService = pythonExecuteService;
    }

    [HttpPost("{symbol}/{startDate}/{endDate}")]
    public async Task<IActionResult> GetStockHistory(string symbol, string startDate, string endDate)
    {
        await _pythonExecuteService.GetStockHistory(symbol, startDate, endDate);
        return Ok();
    }
}