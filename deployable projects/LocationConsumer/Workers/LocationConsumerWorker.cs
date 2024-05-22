using Confluent.Kafka;
using LocationConsumer.Repo;
using Microsoft.Extensions.Hosting;
using SharedModels;

namespace LocationConsumer.Workers;

public class LocationConsumerWorker : BackgroundService
{
    private const string LocationsTopicName = "GPS_Locations";
    private readonly IConsumer<String, CoordinateMessage> _consumer;
    private readonly ILocationRepo _repo;
    public LocationConsumerWorker(IConsumer<String, CoordinateMessage> consumer, ILocationRepo repo)
    {
        _consumer = consumer;
        _repo = repo;
    }
    
    protected async Task HandleMessage(CoordinateMessage message, CancellationToken cancellationToken)
    {
        //TODO update to fit models
        var locations = new List<Location>()
        {
            new Location()
            {
                Timestamp = message.Timestamp,
                Latitude = message.Coordinate.Latitude,
                Longitude = message.Coordinate.Longitude,
                TractorId = message.VehicleId,
                CompanyId = new Guid("00000000-0000-0000-0000-000000000000")
            }
        };
        _repo.BatchInsert(locations);
        
        
        
        Console.WriteLine(message);
        await Task.CompletedTask;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(LocationsTopicName);
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(stoppingToken);
            await HandleMessage(result.Message.Value, stoppingToken);
        }
        
        _consumer.Close();
        await Task.Delay(1000);
    }
}