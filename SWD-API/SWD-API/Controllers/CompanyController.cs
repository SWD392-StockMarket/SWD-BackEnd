using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Company;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Get a paginated list of companies with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCompanies([FromQuery] string? searchTerm, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var companies = await _companyService.GetCompaniesAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving companies.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get company by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                return Ok(company);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Company with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the company.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new company
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid company data." });

            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(dto);
                return Ok(createdCompany);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the company.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing company
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid company data." });

            try
            {
                var updatedCompany = await _companyService.UpdateCompanyAsync(id, dto);
                return Ok(updatedCompany);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Company with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the company.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a company by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Company with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the company.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all stocks of a company
        /// </summary>
        [HttpGet("{id}/stocks")]
        public async Task<IActionResult> GetCompanyStocks(int id)
        {
            try
            {
                var stocks = await _companyService.GetCompanyStocksAsync(id);
                if (stocks == null || stocks.Count == 0)
                    return NotFound(new { Message = $"No stocks found for company with ID {id}." });

                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving company stocks.", Error = ex.Message });
            }
        }
    }
}
