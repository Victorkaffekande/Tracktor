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
    [Route("LocationFromTractor")]
    public IActionResult GetLocationFromTractor(Guid vehicleId, string weekYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
       

        return Ok(_locationApiService.GetLocationFromTractor(vehicleId,weekYear,fromTimestamp,toTimestamp));
    }
    
    [HttpGet]
    [Route("LocationFromCompany")]
    public IActionResult GetLocationFromCompany(Guid fleetId, DateTime fromTimestamp, DateTime toTimestamp,string hourDate)
    {
       
        return Ok(_locationApiService.GetLocationFromCompany(fleetId,fromTimestamp,toTimestamp,hourDate ));
    }

    
    
    [HttpGet]
    [Route("TestCompany")]
    public IActionResult TestCompany()
    {
        return Ok(_locationApiService.TestCompany());
    }

    [HttpGet]
    [Route("TestTractor")]
    public IActionResult TestTractor()
    {
        return Ok(_locationApiService.TestTractor());
    }
}