using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaleSystem.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleSystem.DAL.Repository.contract;
using SaleSystem.DAL.Repository;
using SaleSystem.Utility; 

namespace SaleSystem.IOC
{
    public static class Dependencie
    {
        public static void Injectdependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("stringSQL"));
            });
            // any model
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // especific model
            services.AddScoped<ISaleRepository, SaleRepository>();
            // all mapper profile
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }    
    }
}
