using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SaleSystem.DAL.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly DbContext _dbcontext;

        public GenericRepository(DbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Add(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Delete(TModel model)
        {
            try
            {

            }
            catch
            {
                throw;
            }
        }

        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel model = await _dbcontext.Set<TModel>().FirstOrDefaultAsync(filter);
                return model;
            }
            catch
            {
                throw;
            }
        }

        public Task<IQueryable<TModel>> Query(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
