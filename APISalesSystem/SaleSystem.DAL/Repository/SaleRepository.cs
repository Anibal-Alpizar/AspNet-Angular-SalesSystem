using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DAL.DBContext;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.Model;


namespace SaleSystem.DAL.Repository
{
    public class SaleRepository : GenericRepository<Venta>, ISaleRepository
    {
        private readonly DbventaContext _dbcontext;

        public SaleRepository(DbventaContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Register(Venta model)
        {
            Venta ventaGenerate = new Venta();

            using(var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach(DetalleVenta dv in model.DetalleVenta)
                    {
                        Producto find_product = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        find_product.Stock = find_product.Stock - dv.Cantidad;
                        _dbcontext.Productos.Update(find_product);
                    }
                    await _dbcontext.SaveChangesAsync();
                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;
                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    int numberDigits = 4;
                    string ceros = String.Concat(Enumerable.Repeat("0", numberDigits));
                    string numberSale = ceros + correlativo.UltimoNumero.ToString();
                    
                    // 0001
                    numberSale = numberSale.Substring(numberSale.Length - numberDigits, numberDigits);
                    model.NumeroDocumento = numberSale;
                    await _dbcontext.Venta.AddAsync(model);
                    await _dbcontext.SaveChangesAsync();
                    ventaGenerate = model;
                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                return ventaGenerate;
            }
        }
    }
}
