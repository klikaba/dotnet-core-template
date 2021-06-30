﻿using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Klika.AuthApi.Configuration;
using Klika.AuthApi.Database.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Klika.AuthApi.Extensions.Startup
{
    public static class InitializeIdentityDbExtension
    {
        public async static Task UseIdentityServerDataAsync(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync()
                                                                                                   .ConfigureAwait(false);

                await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync()
                                                                                                   .ConfigureAwait(false);


                var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await configurationContext.Database.MigrateAsync()
                                               .ConfigureAwait(false);

                //Get configuration from db
                var dbClients = await configurationContext.Clients.ToListAsync().ConfigureAwait(false);
                var dbApiScopes = await configurationContext.ApiScopes.ToListAsync().ConfigureAwait(false);
                var dbApiResources = await configurationContext.ApiResources.ToListAsync().ConfigureAwait(false);
                var dbIdentityResources = await configurationContext.IdentityResources.ToListAsync().ConfigureAwait(false);

                //check if some configuration is missing in database if yes insert into database
                foreach (var client in IdentityConfiguration.GetClients())
                {
                    if (!dbClients.Any(x => x.ClientId == client.ClientId))
                        configurationContext.Clients.Add(client.ToEntity());
                }

                foreach (var scope in IdentityConfiguration.GetApiScopes())
                {
                    if (!dbApiScopes.Any(x => x.Name == scope.Name))
                        configurationContext.ApiScopes.Add(scope.ToEntity());
                }

                foreach (var apiResource in IdentityConfiguration.GetResourceApis())
                {
                    if (!dbApiResources.Any(x => x.Name == apiResource.Name))
                        configurationContext.ApiResources.Add(apiResource.ToEntity());
                }

                foreach (var identityResource in IdentityConfiguration.GetIdentityResources())
                {
                    if (!dbIdentityResources.Any(x => x.Name == identityResource.Name))
                        configurationContext.IdentityResources.Add(identityResource.ToEntity());
                }

                await configurationContext.SaveChangesAsync()
                                          .ConfigureAwait(false);

                // seed the Identity Roles (define here which roles application will have)
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.Roles.Any(r => r.Name.Equals("admin")))
                    await roleManager.CreateAsync(new IdentityRole("admin"))
                                     .ConfigureAwait(false);
            }
        }
    }
}
