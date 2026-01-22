using UserService.API.Configurations;
using UserService.BLL.Users.Commands.CreateUser;
using UserService.DAL.Configurations;

namespace UserService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.ConfigureLogging();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.ConfigureDalDependencies();
        
        builder.Services.ConfigureMediator();
        
        builder.Services.ConfigureMapper();
        
        builder.Services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });
        
        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        
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
