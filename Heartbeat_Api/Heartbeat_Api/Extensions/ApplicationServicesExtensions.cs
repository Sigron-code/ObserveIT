using Heartbeat_Api.Data;
using Heartbeat_Api.Interfaces;
using Heartbeat_Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Heartbeat_Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            //services.AddDbContext<DataContext>(options =>
            //{
            //    options.UseSqlite(config.GetConnectionString("DefaultConnection")),
            //    ServiceLifetime.Singleton;
            //});

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(config.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Singleton);




            return services;
        }
    }
}
