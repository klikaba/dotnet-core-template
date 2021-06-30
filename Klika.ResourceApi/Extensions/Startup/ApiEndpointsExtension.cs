using Microsoft.AspNetCore.Builder;
namespace Klika.ResourceApi.Extensions.Startup
{
    public static class ApiEndpointsExtension
    {
        public static void UseApiEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
