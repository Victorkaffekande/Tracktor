using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace LocationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationProducerController : ControllerBase
{
    
    private const string LocationsTopicName = "GPS_Locations";
    private IProducer<String, LocationMessage> _producer;

    public LocationProducerController(IProducer<String, LocationMessage> producer)
    {
        _producer = producer;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(LocationMessage locationMessage)
    {
        var message = new Message<String, LocationMessage>
        {
            Key = locationMessage.VehicleId.ToString(),
            Value = locationMessage
        };

        var res = await _producer.ProduceAsync(LocationsTopicName, message);
        _producer.Flush();
        return Ok(res.Value);
    }
    
    
    
    
    
    
    
}