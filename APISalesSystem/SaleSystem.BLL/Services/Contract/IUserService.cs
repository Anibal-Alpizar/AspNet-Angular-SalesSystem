using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DTO;

namespace SaleSystem.BLL.Services.Contract
{
    public interface IUserService
    {
        Task<List<UsuarioDTO>>List();
        Task<SesionDTO>ValidateCredentials(string email, string password);
        Task<UsuarioDTO> Create(UsuarioDTO model);
        Task<bool> Update(UsuarioDTO model);
        Task<bool> Delete(int id);
    }
}
