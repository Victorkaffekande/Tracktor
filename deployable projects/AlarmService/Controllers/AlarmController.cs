using System.Text.Json;
using AlarmService.client;
using AlarmService.Helper;
using AlarmService.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AlarmService.Controllers;

[ApiController]
[Route("[controller]")]
public class AlarmController : ControllerBase
{
    private readonly RedisClient _redisClient;

    public AlarmController(RedisClient redisClient)
    {
        _redisClient = redisClient;
        _redisClient.Connect();
    }

    [HttpPost]
    public void SaveData([FromBody] FenceRequest fenceRequest)
    {
        var fenceString = JsonSerializer.Serialize(fenceRequest.Fences);
        _redisClient.StoreString(fenceRequest.TractorId, fenceString);
    }

    [HttpGet]
    [Route("checkPoint")]
    public bool GetData([FromQuery(Name = "x")] double x, [FromQuery(Name = "y")] double y,
        [FromQuery(Name = "tractorId")] string id)
    {
        var point = new GeoPoint
        {
            X = x,
            Y = y
        };
        var geometryHelper = new GeometryHelper(_redisClient);
        return geometryHelper.IsPointValid(id, point);
    }


    [HttpGet]
    [Route("get")]
    public string? GetData([FromQuery(Name = "id")] string id)
    {
        return _redisClient.GetString(id);
    }

    [HttpDelete]
    public void Delete(string id)
    {
        _redisClient.RemoveString(id);
    }
}