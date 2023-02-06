using System;
using System.Collections.Generic;
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
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Usuario> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<Usuario> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> List()
        {
            try
            {
                var queryUser = await _userRepository.Query();
                var listUsers = queryUser.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listUsers);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidateCredentials(string email, string password)
        {
            try
            {
                var queryUser = await _userRepository.Query(u => u.Correo == email && u.Clave == password);
                if (queryUser.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe");
                Usuario returnUser = queryUser.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(returnUser);
            }
            catch
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Create(UsuarioDTO model)
        {
            try
            {
                var userCreated = await _userRepository.Create(_mapper.Map<Usuario>(model));
                if (userCreated.IdUsuario == 0)
                    throw new TaskCanceledException("El usuario no se pude crear");
                var query = await _userRepository.Query(u => u.IdUsuario == userCreated.IdUsuario);
                userCreated = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(userCreated);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userFound = await _userRepository.Get(u => u.IdUsuario == id);
                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe");
                bool res = await _userRepository.Delete(userFound);
                if (!res)
                    throw new TaskCanceledException("No se pudo eliminar");
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(UsuarioDTO model)
        {
            try
            {
                var userModel = _mapper.Map<Usuario>(model);
                var userFound = await _userRepository.Get(u => u.IdUsuario == userModel.IdUsuario);
                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe");
                userFound.NombreCompleto = userModel.NombreCompleto;
                userFound.Correo = userModel.Correo;
                userFound.IdRol = userModel.IdRol;
                userFound.Clave = userModel.Clave;
                userFound.EsActivo = userModel.EsActivo;
                bool res = await _userRepository.Update(userFound);
                if (!res)
                    throw new TaskCanceledException("No se pudo editar");
                return res;
            }
            catch
            {
                throw;
            }
        }
    }
}
