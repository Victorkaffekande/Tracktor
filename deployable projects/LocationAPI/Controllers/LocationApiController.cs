using LocationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class LocationApiController : ControllerBase
{
    private readonly ILocationApiService _locationApiService;

    public LocationApiController(ILocationApiService locationApiService)
    {
        _locationApiService = locationApiService;
    }
    
    
    [HttpGet]
    [Route("LocationFromVehicle")]
    public IActionResult GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
        return Ok(_locationApiService.GetLocationFromVehicle(vehicleId,weekYear,fromTimestamp,toTimestamp));
    }
    
    [HttpGet]
    [Route("LocationFromFleet")]
    public IActionResult GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
       
        return Ok(_locationApiService.GetLocationFromFleet(fleetId,fromTimestamp,toTimestamp,hourDate ));
    }

    
    
    [HttpGet]
    [Route("TestFleet")]
    public IActionResult TestFleet()
    {
        return Ok(_locationApiService.TestFleet());
    }

    [HttpGet]
    [Route("TestVehicle")]
    public IActionResult TestVehicle()
    {
        return Ok(_locationApiService.TestVehicle());
    }
}