using Microsoft.Extensions.DependencyInjection;
using Transversal.Configurations;
using Transversal.Interfaces;
using Transversal.Security;

namespace Transversal;

public static class ServicesExtension
{
    public static void AddTransversalServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<JwtOptions>();
    }
}