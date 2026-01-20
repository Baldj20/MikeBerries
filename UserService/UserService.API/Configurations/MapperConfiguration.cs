using UserService.API.Mapping;

namespace UserService.API.Configurations;

public static class MapperConfiguration
{
    public static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CommandProfiles).Assembly);
    }
}
