
using SWD.Data.DTOs;
using SWD.Data.DTOs.Company;
using SWD.Data.DTOs.Stock;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IStockRopository _stockRepository;

        public CompanyService(ICompanyRepository companyRepository, IStockRopository stockRepository)
        {
            _companyRepository = companyRepository;
            _stockRepository = stockRepository;
        }

        public async Task<PageListResponse<CompanyDTO>> GetCompaniesAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var companies = await _companyRepository.GetAllAsync(includeProperties: "Stocks");

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                companies = companies.Where(c => c.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    companies = companies.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    companies = companies.OrderBy(GetSortProperty(sortColumn)).ToList();
                }
            }

            var totalCount = companies.Count();

            // Apply pagination
            var paginatedCompanies = companies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<CompanyDTO>
            {
                Items = MapCompaniesToDTOs(paginatedCompanies),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<CompanyDTO> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepository.GetAsync(c => c.CompanyId == id)
                          ?? throw new KeyNotFoundException("Company not found.");

            return new CompanyDTO
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                Ceo = company.Ceo,
                Information = company.Information
            };
        }

        public async Task<CompanyDTO> CreateCompanyAsync(CreateCompanyDTO dto)
        {
            var company = new Company
            {
                CompanyName = dto.CompanyName,
                Ceo = dto.Ceo,
                Information = dto.Information
            };

            await _companyRepository.AddAsync(company);
            return new CompanyDTO
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                Ceo = company.Ceo,
                Information = company.Information
            };
        }

        public async Task<CompanyDTO> UpdateCompanyAsync(int id, UpdateCompanyDTO dto)
        {
            var company = await _companyRepository.GetAsync(c => c.CompanyId == id)
                          ?? throw new KeyNotFoundException("Company not found.");

            company.CompanyName = dto.CompanyName;
            company.Ceo = dto.Ceo;
            company.Information = dto.Information;

            var updatedCompany = await _companyRepository.UpdateAsync(company);
            return new CompanyDTO
            {
                CompanyId = updatedCompany.CompanyId,
                CompanyName = updatedCompany.CompanyName,
                Ceo = updatedCompany.Ceo,
                Information = updatedCompany.Information
            };
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _companyRepository.GetAsync(c => c.CompanyId == id);
            if (company == null) return false;

            await _companyRepository.DeleteAsync(company);
            return true;
        }

        public async Task<List<StockDTO>> GetCompanyStocksAsync(int companyId)
        {
            var stocks = await _stockRepository.GetAllAsync(s => s.CompanyId == companyId);
            return stocks.Select(s => new StockDTO
            {
                StockId = s.StockId,
                StockSymbol = s.StockSymbol,
                MarketId = s.MarketId,
                ListedDate = s.ListedDate,
                CompanyId = s.CompanyId
            }).ToList();
        }

        private static Func<Company, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "companyname" => company => company.CompanyName,
                "ceo" => company => company.Ceo,
                _ => company => company.CompanyId
            };
        }
        private List<CompanyDTO> MapCompaniesToDTOs(List<Company> companies)
        {
            return companies.Select(c => new CompanyDTO
            {
                CompanyId = c.CompanyId,
                CompanyName = c.CompanyName,
                Ceo = c.Ceo,
                Information = c.Information
            }).ToList();
        }
    }
}
