using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarParkFinder.Infrastructure.Persistence;
using CarParkFinder.Domain.Entities;

[Route("api/[controller]")]
[ApiController]
public class CarParkController : ControllerBase
{
    private readonly CarParkFinderDbContext _context;

    public CarParkController(CarParkFinderDbContext context)
    {
        _context = context;
    }

    // GET: api/CarPark
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarPark>>> GetCarParks()
    {
        return await _context.CarParks.ToListAsync();
    }

    // GET: api/CarPark/{car_park_no}
    [HttpGet("{car_park_no}")]  // This should match the parameter name
    public async Task<ActionResult<CarPark>> GetCarPark(string car_park_no)
    {
        var carPark = await _context.CarParks.FindAsync(car_park_no);
        if (carPark == null)
        {
            return NotFound();
        }
        return carPark;
    }
}
