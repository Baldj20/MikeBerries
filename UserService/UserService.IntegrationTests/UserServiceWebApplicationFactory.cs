using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using UserService.API;
using UserService.DAL;

namespace UserService.IntegrationTests;

public class UserServiceWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage(PostgreSqlTestDatabaseCredentials.ImageName)
        .WithDatabase(PostgreSqlTestDatabaseCredentials.DatabaseName)
        .WithUsername(PostgreSqlTestDatabaseCredentials.Username)
        .WithPassword(PostgreSqlTestDatabaseCredentials.Password)
        .Build();

    private string ConnectionString => _container.GetConnectionString();


    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserServiceDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<UserServiceDbContext>(options => 
                options.UseNpgsql(ConnectionString));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
        db.Database.Migrate();

        return host;
    }
}
