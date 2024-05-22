using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace LocationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationProducerController : ControllerBase
{
    
    private const string LocationsTopicName = "GPS_Locations";
    private IProducer<String, CoordinateMessage> _producer;

    public LocationProducerController(IProducer<String, CoordinateMessage> producer)
    {
        _producer = producer;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CoordinateReading coordinateReading)
    {
        var message = new Message<String, CoordinateMessage>
        {
            Key = coordinateReading.VehicleId.ToString(),
            Value = new CoordinateMessage
            {
                VehicleId = coordinateReading.VehicleId,
                Coordinate = coordinateReading.Coordinate,
                Timestamp = DateTime.Now
            }
        };
        //TODO figure out how to see if message didn't send, and handle failed messages? (Maybe)
        var res = await _producer.ProduceAsync(LocationsTopicName, message);
        _producer.Flush();
        return Ok(res.Value);
    }
    
    
    
    
    
    
    
}