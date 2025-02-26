
using System.ComponentModel.DataAnnotations;
namespace SWD.Data.DTOs.Company
{
    public class UpdateCompanyDTO
    {
        [Required]
        public string CompanyName { get; set; } = null!;

        public string? Ceo { get; set; }

        public string? Information { get; set; }
    }
}
