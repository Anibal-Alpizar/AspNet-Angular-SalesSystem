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
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Producto> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Producto> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductoDTO> Create(ProductoDTO model)
        {
            try
            {
                var productCreated = await _productRepository.Create(_mapper.Map<Producto>(model));
                if (productCreated.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear");
                return _mapper.Map<ProductoDTO>(productCreated);
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
                var foundProduct = await _productRepository.Get(p => p.IdProducto == id);
                if(foundProduct == null)
                    throw new TaskCanceledException("El producto no existe");
                bool res = await _productRepository.Delete(foundProduct);
                if (!res)
                    throw new TaskCanceledException("No se pudo eliminar");
                return res;

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ProductoDTO>> List()
        {
            try
            {
                var queryProduct = await _productRepository.Query();
                var productList = queryProduct.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(productList.ToList());
            }
            catch
            {
                throw;
            };
        }

        public async Task<bool> Update(ProductoDTO model)
        {
            try
            {
                var productModel = _mapper.Map<Producto>(model);
                var foundProduct = await _productRepository.Get(u => u.IdProducto == productModel.IdProducto);
                if(foundProduct == null)
                    throw new TaskCanceledException("El producto no existe");
                foundProduct.Nombre = productModel.Nombre;
                foundProduct.IdCategoria = productModel.IdCategoria;
                foundProduct.Stock = productModel.Stock;
                foundProduct.Precio = productModel.Precio;
                foundProduct.EsActivo = productModel.EsActivo;
                bool res = await _productRepository.Update(foundProduct);
                if(!res)
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
