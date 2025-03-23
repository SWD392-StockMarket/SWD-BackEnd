using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services;

public class PythonExecuteService : IPythonExecuteService
{
    private readonly IStockInSessionRepository _stockInSessionRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IStockRopository _stockRepository;

    public PythonExecuteService(IStockInSessionRepository stockInSessionRepository, ISessionRepository sessionRepository, IStockRopository stockRepository)
    {
        _stockInSessionRepository = stockInSessionRepository;
        _sessionRepository = sessionRepository;
        _stockRepository = stockRepository;
    }
    
    public async Task GetStockHistory(string symbol, string startDate, string endDate)
    {
        
        var session = new Session
        {
            SessionType = "History",
            StartTime = DateTime.Parse(startDate),
            EndTime = DateTime.Parse(endDate),
            Status = "Past"
        };

        var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
        
        await _sessionRepository.AddAsync(session);
        
// Get the base directory (bin/Debug/net8.0/)
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

// Navigate to the correct root folder (4 levels up instead of 3)
        string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\")); 

// Combine with "venv\Scripts\python.exe"
        string pythonPath = Path.Combine(projectRoot, "venv", "Scripts", "python.exe");

        Console.WriteLine("Python Executable Path: " + pythonPath);

        // Combine with "SWD.Service\PythonScripts\StockHistory.py"
        string scriptPath = Path.Combine(projectRoot, "SWD.Service", "PythonScripts", "StockHistory.py");

        if (!File.Exists(pythonPath))
        {
            Console.WriteLine("Python executable not found at: " + pythonPath);
            return;
        }
        if (!File.Exists(scriptPath))
        {
            Console.WriteLine("Script file not found at: " + scriptPath);
            return;
        }

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = pythonPath,
            Arguments = $"\"{scriptPath}\" {symbol} {startDate} {endDate}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        psi.EnvironmentVariables["PYTHONUTF8"] = "1";
        
        Process process = new Process { StartInfo = psi };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        int jsonStart = output.IndexOf("[");
        if (jsonStart != -1)
        {
            output = output.Substring(jsonStart); // Cắt bỏ phần đầu quảng cáo
        }
        
        try
        {
            // Parse JSON output dynamically
            JsonArray jsonArray = JsonNode.Parse(output)?.AsArray() ?? new JsonArray();


            foreach (var item in jsonArray)
            {
                string dateTime = item["time"]?.ToString() ?? "";
                string openPrice = item["open"]?.ToString() ?? "";
                string closePrice = item["close"]?.ToString() ?? "";
                string highPrice = item["high"]?.ToString() ?? "";
                string lowPrice = item["low"]?.ToString() ?? "";
                string volume = item["volume"]?.ToString() ?? "";

                // Convert to DateTime (UTC)
                DateTime utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(dateTime)).UtcDateTime;
                
                var stockInSession = new StockInSession
                {
                    StockId = stock.StockId,
                    SessionId = session.SessionId,
                    DateTime = utcDateTime,
                    OpenPrice = decimal.Parse(openPrice),
                    ClosePrice = decimal.Parse(closePrice),
                    HighPrice = decimal.Parse(highPrice),
                    LowPrice = decimal.Parse(lowPrice),
                    
                };
                await _stockInSessionRepository.AddAsync(stockInSession);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error parsing JSON: " + ex.Message);
        }
    }
}