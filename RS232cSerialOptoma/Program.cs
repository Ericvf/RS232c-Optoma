using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RS232cSerialOptoma
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var application = serviceProvider.GetRequiredService<App>();
            await application.RunAsync(args);
        }

        private static ServiceProvider BuildServiceProvider()
            => new ServiceCollection()
                .AddLogging(logging => logging.AddDebug())
                .AddSingleton<App>()
                .AddSingleton<IRS232cSerialOptomaClient, RS232cSerialOptomaClient>()
                .BuildServiceProvider();
    }
}