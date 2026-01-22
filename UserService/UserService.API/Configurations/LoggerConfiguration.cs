using NLog;
using NLog.Web;

namespace UserService.API.Configurations;

public static class LoggerConfiguration
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        
        LogManager.Setup().LoadConfiguration(config => {
            config.ForLogger()
                .FilterMinLevel(NLog.LogLevel.Info)
                .WriteToConsole();
        });
    }
}
