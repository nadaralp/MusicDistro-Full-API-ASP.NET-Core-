using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicDistory.Data;
using MusicDistro.Api.Configuration;
using MusicDistro.Api.Settings;
using MusicDistro.Core.Entities.Auth;
using MusicDistro.Services;


[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace MusicDistro.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

            // Telling identity to use our DbContext for when we use its services.
            services.AddIdentity<User, Role>(options => // adds User and Role identity + configuration
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }) 
                .AddEntityFrameworkStores<MusicDbContext>() // Tells identity where our identity information is stored
                .AddDefaultTokenProviders(); // adds the default provider to generate tokens for password reset, 2-factor auth, change email...

            // Configuring a IOptions to use with DI
            services.Configure<JwtSettings>(Configuration.GetSection(JwtSettings.Jwt));

            var jwtSettings = Configuration.GetSection(JwtSettings.Jwt).Get<JwtSettings>(); // binds IConfigurationSection to the specified section
            services.AddJwtAuth(jwtSettings);

            services.AddAutoMapper(typeof(Startup));

            services.AddDataLayer(Configuration.GetConnectionString("MusicDatabase"));
            services.AddServiceLayer();

            services.AddSwaggerConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerMiddleware();

            app.UseRouting();

            app.UseAuth(); // Will enable Authentication and Authorization middleware

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
