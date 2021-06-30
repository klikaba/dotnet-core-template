using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;

namespace Klika.ResourceApi.Extensions.Startup
{
    public static class AuthApiExtension
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication()
              .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
              {
                  options.Authority = $"{config["AuthApi:Authority"]}";
                  options.ApiName = $"{config["AuthApi:Audience"]}";
                  options.RequireHttpsMetadata = true;
                  options.RoleClaimType = ClaimTypes.Role; // enable role type authorization on endpoints
              });
        }
    }
}
