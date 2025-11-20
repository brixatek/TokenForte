using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using TokenForte.Application.Services;
using TokenForte.Core.Interfaces;
using TokenForte.Infrastructure.Services;

namespace TokenForte.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenForte(this IServiceCollection services)
        {
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddScoped<ITokenForteHmacValidator, TokenForteHmacValidator>();
            services.AddScoped<ITokenForteRsaValidator, TokenForteRsaValidator>();
            services.AddScoped<ITokenFortePssValidator, TokenFortePssValidator>();
            services.AddScoped<ITokenForteEsdsaValidator, TokenForteEsdsaValidator>();
            services.AddScoped<ITokenForteValidator, TokenForteValidator>();
            services.AddScoped<ValidationService>();
            
            return services;
        }
    }
}
