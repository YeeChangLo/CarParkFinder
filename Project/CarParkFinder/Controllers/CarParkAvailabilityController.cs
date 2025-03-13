using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

[Route("api/[controller]")]
[ApiController]
public class CarParkAvailabilityController : ControllerBase
{
    private readonly CarParkAvailabilityService _availabilityService;

    public CarParkAvailabilityController(CarParkAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<ActionResult<JObject>> GetCarParkAvailability()
    {
        var availabilityData = await _availabilityService.GetCarParkAvailabilityAsync();
        return Ok(availabilityData);
    }
}
