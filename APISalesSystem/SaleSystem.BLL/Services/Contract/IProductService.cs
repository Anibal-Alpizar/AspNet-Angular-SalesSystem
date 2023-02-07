using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DTO;

namespace SaleSystem.BLL.Services.Contract
{
    public interface IProductService
    {
        Task<List<ProductoDTO>> List();
        Task<ProductoDTO> Create(ProductoDTO model);
        Task<bool> Update(ProductoDTO model);
        Task<bool> Delete(int id);
    }
}
