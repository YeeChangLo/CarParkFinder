using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.Domain.Entities;
using CarParkFinder.Application.Helpers;
using AutoMapper;
using CarParkFinder.Domain.DTOs;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class CarParkController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CarParkController> _logger;
    private readonly IMapper _mapper;

    public CarParkController(AppDbContext context, ILogger<CarParkController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    // GET: api/CarParks
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CarParkDto>>> GetCarParks()
    {
        var carParks = await _context.CarParks.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<CarParkDto>>(carParks));
    }

    // GET: api/CarPark/car_park_no
    [HttpGet("car_park_no")]
    public async Task<ActionResult<CarPark>> GetCarPark(string car_park_no)
    {
        var carPark = await _context.CarParks.FindAsync(car_park_no);
        var carParkDtos = _mapper.Map<CarParkDto>(carPark);

        return carPark == null ? NotFound() : Ok(carParkDtos);
    }

    // GET: api/CarPark/address
    [HttpGet("address")]
    public async Task<IActionResult> GetCarparkByAddress([FromQuery] string address)
    {
        var carPark = await _context.CarParks
            .Where(ca => EF.Functions.Like(ca.address, $"%{address}%"))
            .OrderByDescending(ca => ca.address)
            .ToListAsync();

        if (!carPark.Any())
        {
            return NotFound(new { message = "No car parks found for the given address." });
        }

        var carParkDtos = _mapper.Map<List<CarParkDto>>(carPark);

        return Ok(carParkDtos);
    }

    // GET: api/CarPark/car_park_type
    [HttpGet("car_park_type")]
    public async Task<IActionResult> GetCarParkType()
    {

        var carParks = await _context.CarParks.ToListAsync();

        if (carParks == null || !carParks.Any())
        {
            return NotFound(new { message = "No car parks available." });
        }

        var groupedCarParks = carParks
            .Where(cp => !string.IsNullOrEmpty(cp.car_park_type)) // Avoid null grouping
            .GroupBy(cp => cp.car_park_type)
            .Select(g => new
            {
                CarParkType = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.CarParkType)
            .ToList();

        return Ok(groupedCarParks);
    }

    // GET: api/CarPark/type_of_parking_system
    [HttpGet("type_of_parking_system")]
    public async Task<IActionResult> GetCarParkTypeOfSystem()
    {
        var carParks = await _context.CarParks.ToListAsync();

        if (carParks == null || !carParks.Any())
        {
            return NotFound(new { message = "No car parks available." });
        }

        var groupedCarParks = carParks
            .Where(cp => !string.IsNullOrEmpty(cp.type_of_parking_system)) // Avoid null grouping
            .GroupBy(cp => cp.type_of_parking_system)
            .Select(g => new
            {
                CarParkType = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.CarParkType)
            .ToList();

        return Ok(groupedCarParks);
    }

    // GET: api/CarPark/short_term_parking_period
    [HttpGet("short_term_parking_period")]
    public async Task<IActionResult> GetShortTermParkingPeriod()
    {
        var carParks = await _context.CarParks.ToListAsync();

        if (carParks == null || !carParks.Any())
        {
            return NotFound(new { message = "No data available." });
        }

        var groupedCarParks = carParks
            .Where(cp => !string.IsNullOrEmpty(cp.short_term_parking)) // Avoid null grouping
            .GroupBy(cp => cp.short_term_parking)
            .Select(g => new
            {
                ShortTermParking = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.ShortTermParking)
            .ToList();

        return Ok(groupedCarParks);
    }

    // GET: api/CarPark/short_term_parking_period
    [HttpGet("free_parking_period")]
    public async Task<IActionResult> GetFreeParkingPeriod()
    {
        var carParks = await _context.CarParks.ToListAsync();

        if (carParks == null || !carParks.Any())
        {
            return NotFound(new { message = "No data available." });
        }

        var groupedCarParks = carParks
            .Where(cp => !string.IsNullOrEmpty(cp.free_parking)) // Avoid null grouping
            .GroupBy(cp => cp.free_parking)
            .Select(g => new
            {
                FreeParking = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.FreeParking)
            .ToList();

        return Ok(groupedCarParks);
    }

    // GET: api/CarPark/night_parking
    [HttpGet("night_parking")]
    public async Task<IActionResult> GetNightParking()
    {
        var carParks = await _context.CarParks.ToListAsync();

        if (carParks == null || !carParks.Any())
        {
            return NotFound(new { message = "No night parking available." });
        }

        carParks
            .Where(ca => ca.night_parking == "YES")
            .ToList();

        var carParkDtos = _mapper.Map<List<CarParkDto>>(carParks);

        return Ok(carParkDtos);
    }


    // GET: api/CarPark/available
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableCarparks([FromQuery] int min = 0)
    {
        if (!Common.IsValidInteger(min.ToString()))
            return BadRequest(new { message = "Invalid minimum check." });

        var availableCarParks = await _context.CarParkAvailability
               .Where(ca => ca.lots_available > min)
               .OrderByDescending(ca => ca.lots_available)
               .ToListAsync();

        if (!availableCarParks.Any())
            return NotFound(new { message = "No available car parks at the moment." });

        var carParkDtos = _mapper.Map<List<CarParkAvailabilityDto>>(availableCarParks);

        return Ok(carParkDtos);
    }

    // GET: api/CarPark/nearest?latitude=1.29027&longitude=103.851959&page=1&per_page=10
    [HttpGet("nearest")]
    public async Task<IActionResult> GetNearestCarParks([FromQuery] string latitude, [FromQuery] string longitude,
                                                         [FromQuery] int page = 1, [FromQuery] int per_page = 10)
    {
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

            int total_results = await query.CountAsync();

            var carParksList = await query
                .Skip((page - 1) * per_page)
                .Take(per_page)
                .ToListAsync();

            var carParks = carParksList
                .Select(cp =>
                {
                    // Convert SVY21 to WGS84 (Helper Function)
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
                .ToList();

            return Ok(new { page, per_page, total_results, data = carParks });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching nearest car parks: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/CarPark
    [HttpPost]
    public async Task<ActionResult<CarParkResponseDto>> AddCarpark([FromBody] CarParkDto carParkDto)
    {
        if (carParkDto == null)
            return BadRequest("Invalid car park data.");

        // Log received data for debugging
        _logger.LogInformation("Received Car Park Data: {@carParkDto}", carParkDto);

        // Convert DTO → Entity
        var carPark = _mapper.Map<CarPark>(carParkDto);
        _context.CarParks.Add(carPark);
        await _context.SaveChangesAsync();

        // Reload entity to get latest DB values
        await _context.Entry(carPark).ReloadAsync();

        // Convert Entity → DTO Response
        var response = _mapper.Map<CarParkResponseDto>(carPark);

        return CreatedAtAction(nameof(GetCarPark), new { car_park_no = carPark.car_park_no }, new { message = "Car park added successfully." });
    }

    [HttpDelete("car_park_no")]
    public async Task<IActionResult> DeleteCarPark([FromQuery] string car_park_no)
    {
        var carPark = await _context.CarParks.FindAsync(car_park_no);

        if (carPark == null)
            return NotFound(new { message = "Car park not found." });

        _context.CarParks.Remove(carPark);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Car park deleted successfully." });
    }

}
