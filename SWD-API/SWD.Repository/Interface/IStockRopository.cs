using SWD.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Repository.Interface
{
    public interface IStockRopository 
    {
        Task<ICollection<Stock>> GetAllStocksAsync();
    }
}
