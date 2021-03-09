using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Training
{
    public class PrintTextToConsoleService : IHostedService
    {
        private readonly IPart _part;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public PrintTextToConsoleService(IPart part, IHostApplicationLifetime lifetime)
        {
            _part = part;
            _applicationLifetime = lifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _part.ExecuteAsync().FireAndForgetSafeAsync();
            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
