using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ServicesExtension
{
    public static void AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository.Repository>();
    }
}