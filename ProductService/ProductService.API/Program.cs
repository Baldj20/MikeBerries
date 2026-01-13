using ProductService.API.Configurations;
using ProductService.BLL.Configurations;
using ProductService.BLL.Constants.Resilience;
using ProductService.DAL.Configurations;
using Serilog;

namespace ProductService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Information()
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.ConfigureAutentication(builder.Configuration);

        builder.Services.AddPolicyBasedAuthorization();

        builder.Services.ConfigureDataAccessLayerDependencies(builder.Configuration);

        builder.Services.ConfigureBusinessLogicLayerDependencies(builder.Configuration);

        ApiExtensions.ConfigureMapping();

        builder.Services.ConfigureValidators();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        builder.Services.AddResiliencePipeline(ResilienceConstants.STANDARD_PIPELINE_NAME, builder.Configuration);
        builder.Services.AddCachingResiliencePipeline(ResilienceConstants.CACHING_PIPELINE_NAME);

        

        var app = builder.Build();

        if (!app.Environment.IsEnvironment("IntegrationTesting"))
        {
            app.ApplyMigrations();
        }      

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowFrontend");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
