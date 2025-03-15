using Microsoft.AspNetCore.Mvc;

namespace CarParkFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParkController : ControllerBase
    {
        private readonly CarParkAvailabilityService _carParkService;

        public CarParkController(CarParkAvailabilityService carParkService)
        {
            _carParkService = carParkService;
        }

        [HttpPost("update-availability")]
        public async Task<IActionResult> UpdateAvailability()
        {
            await _carParkService.FetchAndSaveCarParkAvailability();
            return Ok("Car park availability updated.");
        }
    }
}
