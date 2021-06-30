using AutoMapper;
using IdentityServer4.Validation;
using Klika.AuthApi.AutoMapper;
using Klika.AuthApi.Database.DbContexts;
using Klika.AuthApi.Extensions.Startup;
using Klika.AuthApi.Model.Entities;
using Klika.AuthApi.Service.User;
using Klika.AuthApi.Service.User.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Klika.AuthApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private MapperConfiguration _mapperConfig { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => { options.AddPolicy("CORS", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });
            services.AddControllers();
            services.AddAppVersioning();
            services.AddSwaggerConfiguration();
            services.AddAppInsights();

            services.AddDbContextPool<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDbContext")));

            services.AddSingleton(sp => _mapperConfig.CreateMapper());

            services.AddIdentityServerConfiguration(Configuration);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IIdentityUserService<ApplicationUser>, IdentityUserService<ApplicationUser>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfiguration();
            }

            app.UseAppInsights();
            app.UseCors("CORS");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseApiEndpoints();

            app.UseIdentityServer();
            await app.UseIdentityServerDataAsync(Configuration)
                        .ConfigureAwait(false);
        }
    }
}
