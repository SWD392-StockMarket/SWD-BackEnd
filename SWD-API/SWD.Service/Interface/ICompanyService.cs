

using SWD.Data.DTOs.Company;
using SWD.Data.DTOs.Stock;
using SWD.Data.DTOs;

namespace SWD.Service.Interface
{
    public interface ICompanyService
    {
        Task<PageListResponse<CompanyDTO>> GetCompaniesAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<CompanyDTO> GetCompanyByIdAsync(int id);
        Task<CompanyDTO> CreateCompanyAsync(CreateCompanyDTO dto);
        Task<CompanyDTO> UpdateCompanyAsync(int id, UpdateCompanyDTO dto);
        Task<bool> DeleteCompanyAsync(int id);
        Task<List<StockDTO>> GetCompanyStocksAsync(int companyId);
    }
}
