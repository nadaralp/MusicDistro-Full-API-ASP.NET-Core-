using Microsoft.Extensions.DependencyInjection;
using MusicDistro.Core.Services;
using MusicDistro.Services.JWT;

namespace MusicDistro.Services
{
    public static class StartupSetup
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IMusicService, MusicService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
