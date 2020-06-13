using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MusicDistro.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Music API",
                    Description = "",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nadar Alpenidze",
                        Email = "nadaralp16@gmail.com",
                        Url = new Uri("https://nadaralp.com"),
                    },
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    Url = new Uri("https://example.com/license"),
                    //}
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                // use this instead of AddSecurityRequirement for custom swagger authorize (shows which routes needs lock, etc..)
                c.OperationFilter<AuthorizeCheckOperationFilter>();


                //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            },
                //            Scheme = "oauth2",
                //            Name = "Bearer",
                //            In = ParameterLocation.Header,

                //        },
                //        new List<string>()
                //    }
                //});

                // Set the comments path for the Swagger JSON and UI.
                // Don't Forget to Enable swagger XML comments on csproj
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Configure Schema to have full namespace name
            //services.ConfigureSwaggerGen(options =>
            //{
            //    options.CustomSchemaIds(x => x.FullName);
            //});
        }

        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Music API");

                // To serve swagger UI at root of the project
                c.RoutePrefix = string.Empty;
            });
        }



        /// <summary>
        /// This class specifies which routes need to have authorization and therefore locks them on swagger UI
        /// </summary>
        internal class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                context.ApiDescription.TryGetMethodInfo(out var methodInfo);

                if (methodInfo == null)
                    return;

                var hasAuthorizeAttribute = false;

                if (methodInfo.MemberType == MemberTypes.Method)
                {
                    // NOTE: Check the controller itself has Authorize attribute
                    hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                    // NOTE: Controller has Authorize attribute, so check the endpoint itself.
                    //       Take into account the allow anonymous attribute
                    if (hasAuthorizeAttribute)
                        hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                    else
                        hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
                }

                if (!hasAuthorizeAttribute)
                    return;

                if (!operation.Responses.Any(r => r.Key == StatusCodes.Status401Unauthorized.ToString()))
                    operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
                if (!operation.Responses.Any(r => r.Key == StatusCodes.Status403Forbidden.ToString()))
                    operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

                // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
                //       we can also pass through the names of the policies in the string[]
                //       which will indicate which permission you require.
                operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            }
        };
            }
        }
    }
}
