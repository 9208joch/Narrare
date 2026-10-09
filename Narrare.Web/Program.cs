using Narrare.Web.Components;
using Narrare.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Narrare.Infrastructure.Data;
using Narrare.Application;
using Narrare.Web.Services;

namespace Narrare.Web
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<UsersApiService>();
            builder.Services.AddSingleton<CurrentUserService>();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<PostsApiService>();
            builder.Services.AddScoped<CommentsApiService>();
            builder.Services.AddScoped<MenuItemsApiService>();
            builder.Services.AddScoped<PagesApiService>();
            builder.Services.AddScoped<CategoriesApiService>();
            builder.Services.AddScoped<PageContentApiService>();
            builder.Services.AddScoped<SiteStyleApiService>();

            


            builder.Services.AddHttpClient("NarrareApi", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ApiSettings:BaseUrl"]!
                );
            });
            

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
