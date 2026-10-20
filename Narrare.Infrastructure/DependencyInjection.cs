using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Narrare.Application.Interfaces;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Application.Services;
using Narrare.Infrastructure.Configuration;
using Narrare.Infrastructure.Data;
using Narrare.Infrastructure.Repositories;
using Narrare.Infrastructure.Storage;
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
        services.AddScoped(typeof(IRepository<>), 
            typeof(Repository<>));

        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IPageContentRepository, PageContentRepository>();
        services.AddScoped<ISiteStyleRepository, SiteStyleRepository>();
        services.AddScoped<IImageUsageRepository, ImageUsageRepository>();
        services.AddScoped<IUploadedImageRepository, UploadedImageRepository>();
        services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped< IGroupMessageReadStatusRepository,GroupMessageReadStatusRepository>();
        services.AddScoped<IContentReportRepository, ContentReportRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();

        var imageStorageProvider =
            configuration["ImageStorage:Provider"] ?? "Local";

        switch (imageStorageProvider)
        {
            case "Local":
                services.AddScoped<IImageStorageService, LocalImageStorageService>();
                break;

            case "Azure":
                services.AddScoped<IImageStorageService, AzureImageStorageService>();
                break;

            default:
                throw new InvalidOperationException(
                    $"Okänd bildlagringsleverantör: {imageStorageProvider}");
        }

        services.Configure<ImageStorageOptions>(
    configuration.GetSection(ImageStorageOptions.SectionName));

        return services;
    }
}
