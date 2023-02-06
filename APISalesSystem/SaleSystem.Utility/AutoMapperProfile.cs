using System;
using System.Collections.Generic;
using System.Globalization;
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
            dest.RolDescripcion, opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
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
            opt => opt.MapFrom(orgin => orgin.EsActivo == 1 ? true : false));

            #endregion Usuario

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Producto
            CreateMap<Producto, ProductoDTO>()
                .ForMember(dest =>
                dest.DescripcionCategori,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
                )
                .ForMember(dest =>
                dest.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest => dest.EsActivo,
                 opt => opt.MapFrom(orgin => orgin.EsActivo == true ? 1 : 0));

                CreateMap<ProductoDTO, Producto>()
                .ForMember(dest =>
                dest.IdCategoriaNavigation,
                opt => opt.Ignore()
                )
                .ForMember(dest =>
                dest.Precio,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-PE")))
                )
                .ForMember(dest => dest.EsActivo,
                opt => opt.MapFrom(orgin => orgin.EsActivo == 1 ? true : false));
            #endregion Producto

            #region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(dest =>
                dest.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest =>
                dest.FechaRegistro,
                opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );

            CreateMap<VentaDTO, Venta>()
                .ForMember(dest =>
                dest.Total,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
                );
            #endregion Venta

            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
            .ForMember(dest =>
            dest.DescripcionProducto,
            opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
            )
            .ForMember(dest =>
            dest.TotalTexto,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
            );

            CreateMap<DetalleVentaDTO, DetalleVenta>()
            .ForMember(dest =>
            dest.Precio,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-PE")))
            )
            .ForMember(dest =>
            dest.Total,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
            );
            #endregion DetalleVenta

            #region Reporte
            CreateMap<DetalleVenta, ReporteDTO>()
            .ForMember(dest =>
            dest.FechaRegistro,
            opt => opt.MapFrom(origen => origen.IdProductoNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
            )
            .ForMember(dest =>
            dest.NumeroDocumento,
            opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento)
            )
            .ForMember(dest =>
            dest.TipoPago,
            opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago)
            )
            .ForMember(dest =>
            dest.TotalVenta,
            opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-PE")))
            )
            .ForMember(dest =>
            dest.Producto,
            opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
            )
            .ForMember(dest =>
            dest.Precio,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
            )
            .ForMember(dest =>
            dest.Total,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
            );
            #endregion Reporte
        }
    }
}
