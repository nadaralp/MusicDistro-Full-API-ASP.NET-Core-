using Microsoft.Extensions.DependencyInjection;
using MusicDistro.Core.Events.UserActionAudit;
using MusicDistro.Core.Services;
using MusicDistro.Services.Events;
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

            // Audit events
            services.AddScoped<IUserEventService, UserEventService>();
            services.AddScoped<IUserEventSubscriber, UserEventSubscriber>();
        }
    }
}
