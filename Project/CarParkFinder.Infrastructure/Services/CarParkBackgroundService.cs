using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            // This will run the task when the app starts and continue running periodically
            await Task.Delay(5000, stoppingToken); // Delay before starting the first fetch

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var carParkService = scope.ServiceProvider.GetRequiredService<CarParkAvailabilityService>();
                    // Call the method to fetch and save data
                    await carParkService.FetchAndSaveCarParkAvailability();
                }

                // Delay for the next run (1 hour in this case)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
