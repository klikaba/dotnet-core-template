using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
namespace Klika.AuthApi.Extensions.Startup
{
    public static class SwaggerConfigExtension
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Klika Auth Api",
                    Version = "v1",
                    Description = "Klika Auth Service"
                });

                // Bearer token authentication                
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme()
                    {
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    }
                );

                // Make sure swagger UI requires a Bearer token specified                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] { }
                    },
                });
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Klika.Template.Api v1");
            });
        }
    }
}
