using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CarParkAvailabilityService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly ILogger<CarParkAvailabilityService> _logger;

    public CarParkAvailabilityService(HttpClient httpClient, AppDbContext context, ILogger<CarParkAvailabilityService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    public async Task FetchAndSaveCarParkAvailability()
    {
        string url = "https://api.data.gov.sg/v1/transport/carpark-availability";
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            var items = json["items"][0]["carpark_data"];

            foreach (var item in items)
            {
                string carParkNo = item["carpark_number"]?.ToString();
                DateTime updateTime = item["update_datetime"]?.ToObject<DateTime>() ?? DateTime.UtcNow;

                if (!string.IsNullOrEmpty(carParkNo))
                {
                    // Ensure CarPark exists
                    var carPark = await _context.CarParks.FindAsync(carParkNo);
                    if (carPark == null)
                    {
                        _logger.LogWarning($"Car park {carParkNo} not found in CarParks table, skipping...");
                        continue; // Skip if car park is not in CarParks table
                    }

                    var carparkInfoList = item["carpark_info"];

                    foreach (var carparkInfo in carparkInfoList)
                    {
                        var lotType = carparkInfo["lot_type"]?.ToString() ?? "Default";
                        var totalLots = carparkInfo["total_lots"]?.ToObject<int>() ?? 0;
                        var lotsAvailable = carparkInfo["lots_available"]?.ToObject<int>() ?? 0;

                        // Check if record exists for the same car_park_no and lot_type
                        var existingAvailability = await _context.CarParkAvailability
                            .FirstOrDefaultAsync(c => c.car_park_no == carParkNo && c.lot_type == lotType);

                        if (existingAvailability == null)
                        {
                            // Insert new record
                            var newAvailability = new CarParkAvailability
                            {
                                car_park_no = carParkNo,
                                lot_type = lotType,
                                total_lots = totalLots,
                                lots_available = lotsAvailable,
                                update_at = updateTime
                            };
                            _context.CarParkAvailability.Add(newAvailability);
                        }
                        else
                        {
                            // Update existing record
                            existingAvailability.total_lots = totalLots;
                            existingAvailability.lots_available = lotsAvailable;
                            existingAvailability.update_at = updateTime;
                            _context.CarParkAvailability.Update(existingAvailability);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Car park availability updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching car park availability: {ex.Message}");
        }
    }

}
