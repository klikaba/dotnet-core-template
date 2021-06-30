using Microsoft.AspNetCore.Builder;
namespace Klika.AuthApi.Extensions.Startup
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
