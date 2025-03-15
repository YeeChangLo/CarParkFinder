using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CarParkFinder.BackgroundServices
{
    public class CarParkBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CarParkBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var carParkService = scope.ServiceProvider.GetRequiredService<CarParkAvailabilityService>();
                    await carParkService.FetchAndSaveCarParkAvailability();
                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
