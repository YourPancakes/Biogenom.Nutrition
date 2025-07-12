using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Application.Services;
using Biogenom.Nutrition.Application.Validators;
using Biogenom.Nutrition.Infrastructure.Mappers;
using Biogenom.Nutrition.Persistence.Context;
using Biogenom.Nutrition.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Biogenom.Nutrition.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NutritionAssessmentProfile), typeof(ReportProfile));
        
        services.AddValidatorsFromAssemblyContaining<CreateAssessmentRequestValidator>();
        
        services.AddScoped<INutritionAssessmentService, NutritionAssessmentService>();
        services.AddScoped<INutrientReportService, NutrientReportService>();
        services.AddScoped<INutrientBalanceService, NutrientBalanceService>();
        services.AddScoped<IPersonalizedSetService, PersonalizedSetService>();
        services.AddScoped<INutritionScoreCalculator, NutritionScoreCalculator>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Biogenom.Nutrition")));
        
        return services;
    }

    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
        
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("http://localhost:5000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });
        
        return services;
    }
} 