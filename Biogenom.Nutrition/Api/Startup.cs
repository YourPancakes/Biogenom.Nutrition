using Biogenom.Nutrition.Api.Extensions;
using Biogenom.Nutrition.Api.Middlewares;
using Biogenom.Nutrition.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Biogenom.Nutrition.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCustomApiVersioning();

        services.AddHealthChecks();

        services.AddCorsPolicy();

        services.AddDatabase(Configuration);

        services.AddApplicationServices();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Biogenom Nutrition Assessment API",
                Version = "v1",
                Description = "API for nutrition quality assessment functionality"
            });
        });
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biogenom Nutrition Assessment API v1");
            });
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
            await DbSeed.SeedAsync(dbContext);
        }
    }
} 