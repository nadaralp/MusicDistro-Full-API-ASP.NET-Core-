using Microsoft.Extensions.DependencyInjection;
using MusicDistro.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistro.Services
{
    public static class StartupSetup
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IMusicService, MusicService>();
        }
    }
}
