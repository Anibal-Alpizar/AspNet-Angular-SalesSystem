using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SaleSystem.BLL.Services.Contract;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.DTO;
using SaleSystem.Model;

namespace SaleSystem.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> _userRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> userRepository, IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> List(int idUser)
        {
            IQueryable<Usuario> tbUser = await _userRepository.Query(u => u.IdUsuario == idUser);
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepository.Query();
            IQueryable<Menu> tbMenu = await _menuRepository.Query();
            try
            {
                IQueryable<Menu> tbResult = (from u in tbUser
                                             join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                             join m in tbMenu on mr.IdMenu equals m.IdMenu
                                             select m).AsQueryable();
                var menuList = tbResult.ToList();
                return _mapper.Map<List<MenuDTO>>(menuList);
            }
            catch
            {
                throw;
            }
        }

    }
}
