using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SaleSystem.BLL.Services.Contract;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.DTO;
using SaleSystem.Model;

namespace SaleSystem.BLL.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGenericRepository<Producto> _productRepository;
        private readonly IMapper _mapper;

        public DashBoardService(ISaleRepository saleRepository, IGenericRepository<Producto> productRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        private IQueryable<Venta> returnSales(IQueryable<Venta> saleTable, int subtractDay)
        {
            DateTime? lastDate = saleTable.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            lastDate = lastDate.Value.AddDays(subtractDay);
            return saleTable.Where(v => v.FechaRegistro.Value.Date >= lastDate.Value.Date);
        }

        private async Task<int> TotalSalesLastWeek()
        {
            int total = 0;
            IQueryable<Venta> _saleQuery = await _saleRepository.Query();
            if (_saleQuery.Count() > 0)
            {
                var saleTable = returnSales(_saleQuery, -7);
                total = saleTable.Count();
            }
            return total;
        }

        private async Task<string> TotalIncomeLastWeek()
        {
            decimal restult = 0;
            IQueryable<Venta> _saleQuery = await _saleRepository.Query();
            if (_saleQuery.Count() > 0)
            {
                var salesTable = returnSales(_saleQuery, -7);
                restult = salesTable.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(restult, new CultureInfo("es-PE"));
        }

        private async Task<int> TotalProducts()
        {
            IQueryable<Producto> _productQuery = await _productRepository.Query();
            int total = _productQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> SalesLastWeek()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            IQueryable<Venta> _salesQuery = await _saleRepository.Query();
            if (_salesQuery.Count() > 0)
            {
                var salesTable = returnSales(_salesQuery, -7);
                result = salesTable
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key)
                    .Select(dv => new { date = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.date, elementSelector: r => r.total);
            }
            return result;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();
            try
            {
                vmDashBoard.TotalVentas = await TotalSalesLastWeek();
                vmDashBoard.TotalIngresos = await TotalIncomeLastWeek();
                vmDashBoard.TotalProductos = await TotalProducts();
                List<VentasSemanaDTO> saleListWeek = new List<VentasSemanaDTO>();
                foreach(KeyValuePair<string, int> item in await SalesLastWeek())
                {
                    saleListWeek.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashBoard.VentasUltimaSemana = saleListWeek;
            }
            catch
            {
                throw;
            }
            return vmDashBoard;
        }
    }
}
