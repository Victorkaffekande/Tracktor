using LocationRetrievalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocationRetrievalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationRetrievalApi : ControllerBase
{
    private readonly ILocationRetrievalApiService _locationRetrievalApiService;

    public LocationRetrievalApi(ILocationRetrievalApiService locationRetrievalApiService)
    {
        _locationRetrievalApiService = locationRetrievalApiService;
    }
    
    
    [HttpGet]
    [Route("LocationFromVehicle")]
    public IActionResult GetLocationFromVehicle(Guid vehicleId, string weekYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
        return Ok(_locationRetrievalApiService.GetLocationFromVehicle(vehicleId,weekYear,fromTimestamp,toTimestamp));
    }
    
    [HttpGet]
    [Route("LocationFromFleet")]
    public IActionResult GetLocationFromFleet(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
       
        return Ok(_locationRetrievalApiService.GetLocationFromFleet(fleetId,fromTimestamp,toTimestamp,hourDate ));
    }

    
    
    [HttpGet]
    [Route("TestFleet")]
    public IActionResult TestFleet()
    {
        return Ok(_locationRetrievalApiService.TestFleet());
    }

    [HttpGet]
    [Route("TestVehicle")]
    public IActionResult TestVehicle()
    {
        return Ok(_locationRetrievalApiService.TestVehicle());
    }
}