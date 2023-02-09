using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.Model;

namespace SaleSystem.DAL.Repository.contract
{
    public interface ISaleRepository : IGenericRepository<Venta>
    {
        Task<Venta> Register(Venta model);      
    }
}
