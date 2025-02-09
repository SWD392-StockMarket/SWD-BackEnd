using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class Market
    {
        [Key]
        public int MarketID { get; set; }

        [Required, MaxLength(255)]
        public string MarketName { get; set; }

        public string Address { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }

        public ICollection<Stock> Stocks { get; set; }
    }
}
