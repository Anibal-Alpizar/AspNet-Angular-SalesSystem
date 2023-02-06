using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SaleSystem.DTO;
using SaleSystem.Model;

namespace SaleSystem.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
            .ForMember(dest =>
            dest.RolDescripcion, opt => opt.MapFrom(orgin => orgin.IdRolNavigation.Nombre)
            )
            .ForMember(dest => dest.EsActivo,
            opt => opt.MapFrom(orgin => orgin.EsActivo == true ? 1 : 0));

            CreateMap<Usuario, SesionDTO>()
            .ForMember(dest =>
            dest.RolDescripcion,
            opt => opt.MapFrom(orgin => orgin.IdRolNavigation.Nombre)
            );

            CreateMap<UsuarioDTO, Usuario>()
            .ForMember(dest => dest.IdRolNavigation,
            opt => opt.Ignore())
            .ForMember(dest => dest.EsActivo,
            opt => opt.MapFrom(orgin => orgin.EsActivo == 1 ? true: false));

            #endregion Usuario

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria
        }
    }
}
