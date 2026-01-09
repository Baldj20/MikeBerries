using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.DAL.Configurations;

public static class DalConfiguration
{
    public static void ConfigureDalDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserServiceDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
}