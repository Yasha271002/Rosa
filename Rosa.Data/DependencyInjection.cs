using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rosa.Core.Interfaces;
using Rosa.Data.Repository;

namespace Rosa.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddRosaData(this IServiceCollection services, string connection)
    {
        services.AddDbContext<RosaDbContext>(options =>
            options.UseSqlite(connection));

        services.AddTransient<IRequestRepository, RequestRepository>();

        return services;
    }
}