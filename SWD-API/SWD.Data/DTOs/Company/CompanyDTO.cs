

namespace SWD.Data.DTOs.Company
{
    public class CompanyDTO
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Ceo { get; set; }
        public string? Information { get; set; }
    }
}
