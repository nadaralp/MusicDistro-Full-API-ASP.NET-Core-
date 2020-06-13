using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicDistro.Core;
using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
