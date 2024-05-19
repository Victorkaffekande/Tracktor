using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using SharedModels;

namespace LocationConsumer.Workers;

public class LocationConsumerWorker : BackgroundService
{
    private const string LocationsTopicName = "GPS_Locations";
    private readonly IConsumer<String, CoordinateMessage> _consumer;

    public LocationConsumerWorker(IConsumer<String, CoordinateMessage> consumer)
    {
        _consumer = consumer;
    }
    
    protected async Task HandleMessage(CoordinateMessage message, CancellationToken cancellationToken)
    {
        //TODO put message into database
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