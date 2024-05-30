using System.Text.Json;
using AlarmConsumer.client;
using AlarmService.Helper;
using AlarmService.Schema;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using SharedModels;

namespace AlarmConsumer.Workers;

public class AlarmWorker : BackgroundService
{
    private const string LocationsTopicName = "GPS_Locations";
    private readonly IConsumer<String, CoordinateMessage> _consumer;
    private readonly RedisClient _redisClient;

    public AlarmWorker(IConsumer<string, CoordinateMessage> consumer)
    {
        _consumer = consumer;
        _redisClient = RedisClientFactory.CreateClient();
    }

    protected async Task HandleMessage(CoordinateMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonSerializer.Serialize(message));
        
        var c = message.Coordinate;
        if (c == null) await Task.CompletedTask;
        
        
        var fences = GetFencesFromId(message.VehicleId.ToString());
        //get fence from vehicle id
        //ispoint valid (fence, coodinate)

        var valid = GeometryHelper.IsPointValid(fences, c);

        Console.WriteLine(valid);
        
        await Task.CompletedTask;
    }

    private List<GeoFence> GetFencesFromId(string id)
    {
        var fencesString = _redisClient.GetString(id);
        if (fencesString == null) return new List<GeoFence>();

        var opt = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var list = JsonSerializer.Deserialize<List<GeoFence>>(fencesString, opt);
        return list ?? new List<GeoFence>(); //if deserialize fails return empty list
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