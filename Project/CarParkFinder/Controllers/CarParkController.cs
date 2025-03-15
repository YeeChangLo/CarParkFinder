using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.Domain.Entities;
using CarParkFinder.Application.Helpers;
using System.Net.Http;
using Azure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

[Route("api/[controller]")]
[ApiController]
public class CarParkController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CarParkController> _logger;

    public CarParkController(AppDbContext context, ILogger<CarParkController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/CarParks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetCarParks()
    {
        return await _context.CarParks.ToListAsync();
    }

    // GET: api/CarPark/{car_park_no}
    [HttpGet("{car_park_no}")] 
    public async Task<ActionResult<CarPark>> GetCarPark(string car_park_no)
    {
        var carPark = await _context.CarParks.FindAsync(car_park_no);
        if (carPark == null)
        {
            return NotFound();
        }
        return carPark;
    }

    // GET: api/CarPark/{car_park_no}
    [HttpGet("nearest")]
    public async Task<IActionResult> GetNearestCarParks([FromQuery] string latitude, [FromQuery] string longitude
                                            , [FromQuery] int page = 1, [FromQuery] int per_page = 10)
    {
        // Validations
        if (!double.TryParse(latitude, out double lat) || !double.TryParse(longitude, out double lon))
        {
            return BadRequest("Invalid latitude or longitude format.");
        }

        if (page < 1 || per_page < 1)
        {
            return BadRequest("Page and per_page must be greater than 0.");
        }

        try
        {
            var query = _context.CarParks
            .Join(_context.CarParkAvailability,
                cp => cp.car_park_no,
                ca => ca.car_park_no,
                (cp, ca) => new
                {
                    cp.car_park_no,
                    cp.address,
                    cp.y_coord,
                    cp.x_coord,
                    ca.total_lots,
                    ca.lots_available
                })
            .Where(cp => cp.lots_available > 0);

            // Total count before pagination
            int total_results = await query.CountAsync();

            // Apply pagination
            var carParksList = await query
                .Skip((page - 1) * per_page) 
                .Take(per_page)
                .ToListAsync();

            var carParks = carParksList
                .AsEnumerable()
                .Select(cp =>
                {
                    // Convert SVY21 to WGS84
                    (double conv_lat, double conv_long) = SVY21Converter.ConvertSVY21ToWGS84(double.Parse(cp.y_coord.ToString()), double.Parse(cp.x_coord.ToString()));

                    return new CarParkResponseDto
                    {
                        address = cp.address,
                        latitude = conv_lat,
                        longitude = conv_long,
                        total_lots = cp.total_lots,
                        available_lots = cp.lots_available,
                        Distance = DistanceHelper.CalculateDistance(lat, lon, conv_lat, conv_long, true),
                    };
                })
                .OrderBy(cp => cp.Distance)
                .ToList(); ;

            return Ok(new
            {
                page,
                per_page,
                total_results,
                data = carParks
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching nearest car parks: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


}
