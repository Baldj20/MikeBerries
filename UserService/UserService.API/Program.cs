using UserService.API.Configurations;
using UserService.API.Mapping;
using UserService.BLL.Users.Commands.CreateUser;
using UserService.DAL.Configurations;
using UserService.DAL.Repositories;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.ConfigureDalDependencies();

        builder.Services.ConfigureMediator();
        
        builder.Services.ConfigureMapper();
        
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
