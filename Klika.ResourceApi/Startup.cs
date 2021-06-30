using Klika.ResourceApi.Database.DbContexts;
using Klika.ResourceApi.Extensions.Startup;
using Klika.ResourceApi.Model.Interfaces.Template;
using Klika.ResourceApi.Repository.EFCore;
using Klika.ResourceApi.Service.Template;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Klika.ResourceApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddHttpClient<ITemplateService, TemplateService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAuthentication(Configuration);
            services.AddSwaggerConfiguration();
            services.AddAppVersioning();
            services.AddAppInsights();
            services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));
            services.AddCors(options => { options.AddPolicy("CORS", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

            services.AddDbContextPool<TemplateDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("TemplateDbContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TemplateDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfiguration();
            }
            else
            {
                dbContext.Database.Migrate();
            }

            app.UseCors("CORS");
            app.UseAppInsights();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiEndpoints();
        }
    }
}
