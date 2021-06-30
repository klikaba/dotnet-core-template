using IdentityModel.Client;
using Klika.AuthApi.Database.DbContexts;
using Klika.AuthApi.Model.Constants.Assemblies;
using Klika.AuthApi.Model.Entities;
using Klika.AuthApi.Service.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Klika.AuthApi.Extensions.Startup
{
    public static class IdentityServerConfiguration
    {
        public static void AddIdentityServerConfiguration(this IServiceCollection services, IConfiguration _config)
        {
            string identityConnectionString = _config.GetConnectionString("IdentityDbContext");

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
             .AddEntityFrameworkStores<IdentityDbContext>()
             .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddAspNetIdentity<ApplicationUser>()
                    //Configuration Store: clients and resources
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAssemblies.Database));
                    })
                    //Operational Store: tokens, codes etc.
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAssemblies.Database));
                    })
                    .AddProfileService<IdentityProfileService>(); // custom claims 

            //Cache Discovery document HttpClient
            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(_config["AuthApiUrl"], () => factory.CreateClient());
            });
        }
    }
}
