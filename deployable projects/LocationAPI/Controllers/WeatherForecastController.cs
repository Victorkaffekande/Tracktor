using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public void Get()
    {

    }
}