using System;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using Seamstress.VSA.Infrastructure.Data;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureHangfire(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer();

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("AllowReact", builder =>
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        return services;
    }

    public static IServiceCollection ConfigureQuestPdf(this IServiceCollection services)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        return services;
    }
}
