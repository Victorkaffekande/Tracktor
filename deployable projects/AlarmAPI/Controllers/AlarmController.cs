using System.Text.Json;
using AlarmConsumer.client;
using AlarmService.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AlarmAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AlarmController : ControllerBase
{
    private readonly RedisClient _redisClient;
    public AlarmController(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }
    
    
    [HttpPost]
    public IActionResult SaveFence([FromBody] FenceRequest fenceRequest)
    {
        var fenceString = JsonSerializer.Serialize(fenceRequest.Fences);
        _redisClient.StoreString(fenceRequest.TractorId, fenceString);
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult Delete(string id)
    {
        _redisClient.RemoveString(id);
        return Ok();
    }
    
    [HttpGet]
    public IActionResult GetData([FromQuery(Name = "id")] string id)
    {
        return Ok(_redisClient.GetString(id));
    }
}