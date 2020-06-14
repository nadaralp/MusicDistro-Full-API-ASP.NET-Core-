using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicDistory.Data.Repositories;
using MusicDistro.Core;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TDistory.Data.Repositories;

namespace MusicDistory.Data
{
    public static class StartupSetup
    {
        public static void AddDataLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MusicDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserAuditRepository, UserAuditRepository>();

            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
