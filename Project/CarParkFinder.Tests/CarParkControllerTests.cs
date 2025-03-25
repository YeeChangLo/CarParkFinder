using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.Domain.Entities;
using Newtonsoft.Json;
using AutoMapper;

public class CarParkControllerTests
{
    private readonly CarParkController _controller;
    private readonly AppDbContext _context;
    private readonly Mock<ILogger<CarParkController>> _mockLogger;
    private readonly IMapper _mapper;


    public CarParkControllerTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<ILogger<CarParkController>>();
        _controller = new CarParkController(_context, _mockLogger.Object, _mapper);

        _context.CarParks.RemoveRange(_context.CarParks);
        _context.CarParkAvailability.RemoveRange(_context.CarParkAvailability);
        _context.SaveChanges();
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _context.CarParks.AddRange(new List<CarPark>
        {
            new CarPark
            {
                car_park_no = "CP001",
                address = "123 Street A",
                x_coord = "31421.64", // Example SVY21 coordinates
                y_coord = "39823.29",
                car_park_type = "Multi-Storey",
                type_of_parking_system = "Electronic",
                short_term_parking = "WHOLE DAY",
                free_parking = "SUN & PH FR 7AM-10.30PM",
                night_parking = "YES",
                car_park_decks = "3",
                gantry_height = "2.15",
                car_park_basement = "N"
            },
            new CarPark
            {
                car_park_no = "CP002",
                address = "456 Street B",
                x_coord = "32000.32",
                y_coord = "40200.12",
                car_park_type = "Surface",
                type_of_parking_system = "Manual",
                short_term_parking = "NO",
                free_parking = "NO",
                night_parking = "NO",
                car_park_decks = "1",
                gantry_height = "2.0",
                car_park_basement = "Y"
            }
        });

        _context.CarParkAvailability.AddRange(new List<CarParkAvailability>
        {
            new CarParkAvailability
            {
                car_park_no = "CP001",
                lot_type = "C",
                total_lots = 100,
                lots_available = 50,
                update_at = DateTime.UtcNow
            },
            new CarParkAvailability
            {
                car_park_no = "CP002",
                lot_type = "C",
                total_lots = 200,
                lots_available = 150,
                update_at = DateTime.UtcNow
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetCarParks_ReturnsAllCarParks()
    {
        // Act
        var result = await _controller.GetCarParks();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<CarPark>>>(result);
        var carParks = Assert.IsType<List<CarPark>>(actionResult.Value);
        Assert.Equal(2, carParks.Count);
    }

    [Fact]
    public async Task GetCarPark_ReturnsCorrectCarPark()
    {
        // Act
        var result = await _controller.GetCarPark("CP001");

        // Assert
        var actionResult = Assert.IsType<ActionResult<CarPark>>(result);
        var carPark = Assert.IsType<CarPark>(actionResult.Value);
        Assert.Equal("123 Street A", carPark.address);
    }

    [Fact]
    public async Task GetCarPark_ReturnsNotFound_ForInvalidCarPark()
    {
        // Act
        var result = await _controller.GetCarPark("InvalidCP");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetNearestCarParks_ShouldReturnValidResponse()
    {
        string latitude = "32000.32";
        string longitude = "39823.29";

        // Act
        var result = await _controller.GetNearestCarParks(latitude, longitude, 1, 10);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var jsonString = JsonConvert.SerializeObject(actionResult.Value);

        Assert.NotNull(jsonString);
    }


    [Fact]
    public async Task GetNearestCarParks_ReturnsBadRequest_ForInvalidCoordinates()
    {
        // Act
        var result = await _controller.GetNearestCarParks("invalid", "invalid");

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid latitude or longitude format.", actionResult.Value);
    }
}
