
using Microsoft.Extensions.DependencyInjection;
using Narrare.Application;
using Narrare.Infrastructure;
using Narrare.Infrastructure.Data;

namespace Narrare.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registrera tjänster
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Seed:a databasen
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider
                    .GetRequiredService<DataSeeder>();

                await seeder.SeedAsync();
            }

            // Konfigurera HTTP-pipelinen
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
