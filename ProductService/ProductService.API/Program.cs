using ProductService.API.Configurations;
using ProductService.BLL.Configurations;
using ProductService.DAL;
using ProductService.DAL.Configurations;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;
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

        var minioSettings = builder.Configuration.GetSection("Minio").Get<MinioSettings>();
        if (minioSettings == null)
            throw new InvalidOperationException("Minio settings not found");
        builder.Services.AddSingleton(provider => new MinioStorage(minioSettings));
        builder.Services.AddScoped<IFileRepository, MinioFileRepository>();

        builder.Services.ConfigureDataAccessLayerDependencies(builder.Configuration);

        builder.Services.ConfigureBusinessLogicLayerDependencies();

        ApiExtensions.ConfigureMapping();

        builder.Services.ConfigureValidators();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
