using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narrare.Infrastructure.Data;

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

        return services;
    }
}