using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DTO;

namespace SaleSystem.BLL.Services.Contract
{
    public interface ISaleService
    {
        Task<VentaDTO> Register(VentaDTO model);
        Task<List<VentaDTO>> History(string search, string saleNumber, string startdate, string enddate);
        Task<List<ReporteDTO>> Report(string startdate, string enddate);
    }
}
