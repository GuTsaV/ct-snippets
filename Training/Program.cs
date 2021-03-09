using System;
using System.Threading.Tasks;
using commercetools.Api;
using commercetools.Base.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Training
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = GetConfiguration();
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddSingleton<IHostedService, PrintTextToConsoleService>();
                    ConfigureServices(services, configuration);
                    ConfigureExerciseService(services, args);
                });
            await builder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.UseCommercetoolsApi(configuration, "Client");
            var clientConfiguration = configuration.GetSection("Client").Get<ClientConfiguration>();
            Settings.SetCurrentProjectKey(clientConfiguration.ProjectKey);
        }
        
        private static void ConfigureExerciseService(IServiceCollection services, string[] args)
        {
            var runningPart = args != null && args.Length > 0 ? args[0] : "3";
            Type partType = Type.GetType($"Training.Part{runningPart}");
            if (partType != null)
            {
                services.AddSingleton(typeof(IPart), partType);
            }
        }
        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddEnvironmentVariables().
                Build();
        }
    }
}
