using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Infrastructure.Data;
using Narrare.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NarrareDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("NarrareDb")));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}