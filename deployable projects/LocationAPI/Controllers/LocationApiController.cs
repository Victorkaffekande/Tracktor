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
    public IActionResult GetLocationFromTractor(Guid tractorId, string monthYear, DateTime fromTimestamp, DateTime toTimestamp)
    {
       

        return Ok(_locationApiService.GetLocationFromTractor(tractorId,monthYear,fromTimestamp,toTimestamp));
    }
    
    [HttpGet]
    [Route("LocationFromCompany")]
    public IActionResult GetLocationFromCompany(Guid companyId, DateTime fromTimestamp, DateTime toTimestamp,string monthYear)
    {
       
        return Ok(_locationApiService.GetLocationFromCompany(companyId,fromTimestamp,toTimestamp,monthYear ));
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