using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL.Repositories;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Configurations;

public static class DalConfiguration
{
    public static void ConfigureDalDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserServiceDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
    }
}
