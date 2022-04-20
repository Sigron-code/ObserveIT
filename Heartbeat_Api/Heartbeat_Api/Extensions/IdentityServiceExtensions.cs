using Heartbeat_Api.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Heartbeat_Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        //ICalculationService calculationService
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config, ICalculationService calculationService)
            
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                       ValidateIssuer = false,
                       ValidateAudience = false,
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = async (context) =>
                       {
                           Console.WriteLine("Printing in the delegate OnAuthFailed");
                       },
                       OnChallenge = async (context) =>
                       {
                           calculationService.AddUnRegisteredAgents();
                           Console.WriteLine("Printing in the delegate OnChallenge");
                       }
                   };
               });

            return services;
        }
    }
}
