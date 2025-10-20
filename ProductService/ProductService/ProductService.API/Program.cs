using ProductService.API.Configurations;
using ProductService.BLL.Configurations;
using ProductService.DAL.Configurations;

namespace ProductService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.ConfigureDataAccessLayerDependencies(builder.Configuration);

        builder.Services.ConfigureBusinessLogicLayerDependencies();

        builder.Services.ConfigureValidators();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
