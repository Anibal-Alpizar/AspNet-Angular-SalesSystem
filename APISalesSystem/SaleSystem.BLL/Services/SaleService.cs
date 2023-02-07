using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SaleSystem.BLL.Services.Contract;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.DTO;
using SaleSystem.Model;

namespace SaleSystem.BLL.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGenericRepository<DetalleVenta> _saleDetailRepository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository saleRepository, IGenericRepository<DetalleVenta> saleDetailRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _saleDetailRepository = saleDetailRepository;
            _mapper = mapper;
        }

        public async Task<List<VentaDTO>> History(string search, string saleNumber, string startdate, string enddate)
        {
            IQueryable<Venta> query = await _saleRepository.Query();
            var ListResult = new List<Venta>();
            try
            {
                if (search == "fecha")
                {
                    DateTime start_date = DateTime.ParseExact(startdate, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime end_date = DateTime.ParseExact(enddate, "dd/MM/yyyy", new CultureInfo("es-PE"));

                    ListResult = await query.Where(x =>
                        x.FechaRegistro.Value.Date >= start_date.Date &&
                        x.FechaRegistro.Value.Date <= end_date.Date
                        ).Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    ListResult = await query.Where(x => x.NumeroDocumento == saleNumber
                        ).Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
            }
            catch
            {
                throw;
            }
            return _mapper.Map<List<VentaDTO>>(ListResult);
        }

        public async Task<VentaDTO> Register(VentaDTO model)
        {
            try
            {
                var saleGenerated = await _saleRepository.Register(_mapper.Map<Venta>(model));
                if (saleGenerated.IdVenta == 0)
                    throw new Exception("No se pudo crear");
                return _mapper.Map<VentaDTO>(saleGenerated);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ReporteDTO>> Report(string startdate, string enddate)
        {
            IQueryable<DetalleVenta> query = await _saleDetailRepository.Query();
            var ListResult = new List<DetalleVenta>();
            try
            {
                DateTime start_date = DateTime.ParseExact(startdate, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime end_date = DateTime.ParseExact(enddate, "dd/MM/yyyy", new CultureInfo("es-PE"));
                ListResult = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                    dv.IdVentaNavigation.FechaRegistro.Value.Date >= start_date.Date &&
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= end_date.Date
                    ).ToListAsync();
            }
            catch
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListResult);
        }
    }
}
