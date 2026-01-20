using UserService.BLL.Users.Commands.CreateUser;

namespace UserService.API.Configurations;

public static class MediatorConfiguration
{
    public static void ConfigureMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });
    }
}
