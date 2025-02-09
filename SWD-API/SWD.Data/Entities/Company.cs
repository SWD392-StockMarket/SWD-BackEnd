using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required, MaxLength(255)]
        public string CompanyName { get; set; }

        public string CEO { get; set; }
        public string Information { get; set; }

        public ICollection<Stock> Stocks { get; set; }
    }
}
