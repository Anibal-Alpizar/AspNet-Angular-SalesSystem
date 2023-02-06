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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Categoria> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Categoria> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> List()
        {
            try
            {
                var categoryList = await _categoryRepository.Query();
                return _mapper.Map<List<CategoriaDTO>>(categoryList.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}
