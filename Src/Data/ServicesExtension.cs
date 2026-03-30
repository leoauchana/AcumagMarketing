using Data.Context;
using Data.Files;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Transversal.Configurations;

namespace Data;

public static class ServicesExtension
{
    public static void AddDataServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository.Repository>();
        services.AddScoped<IFileStorage, LocalFileStorage>();
        services.AddDbContext<AcumagContext>((sp, options) =>
        {
            var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseSqlServer(dbOptions.SqlConnectionString);
        });
    }
}