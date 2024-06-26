﻿using System.Text.Json;
using AlarmConsumer.client;
using AlarmConsumer.Helper;
using AlarmConsumer.Models;
using AlarmService.Schema;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using SharedModels;

namespace AlarmConsumer.Workers;

public class AlarmWorker : BackgroundService
{
    private const string LocationsTopicName = "GPS_Locations";
    private const string AlarmTopicName = "Alarm";
    private readonly IConsumer<String, LocationMessage> _consumer;
    private readonly IProducer<String, AlarmMessage> _producer;
    private readonly RedisClient _redisClient;

    public AlarmWorker(IConsumer<string, LocationMessage> consumer, IProducer<string, AlarmMessage> producer)
    {
        _consumer = consumer;
        _producer = producer;
        _redisClient = RedisClientFactory.CreateClient();
        _redisClient.Connect();
    }

    protected async Task HandleMessage(LocationMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonSerializer.Serialize(message));

        var c = new Coordinate()
        {
            Longitude = message.Longitude,
            Latitude = message.Latitude
        };

        var fences = GetFencesFromId(message.VehicleId.ToString());

        if (fences.Count == 0)
        {
            await Task.CompletedTask;
        }
        var valid = GeometryHelper.IsPointValid(fences, c);

        if (!valid)
        {
            var alarmMessage = new Message<String, AlarmMessage>
            {
                Key = message.VehicleId.ToString(),
                Value = new AlarmMessage()
                {
                    VehicleId = message.VehicleId,
                    Longitude = message.Longitude,
                    Latitude = message.Latitude,
                    Timestamp = DateTime.Now
                }
            };
            
            var res = await _producer.ProduceAsync(AlarmTopicName, alarmMessage, cancellationToken);
            _producer.Flush(cancellationToken);
        }
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
        if (list == null) throw new Exception("Failed to deserialize fence string");
        
        return list;
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
    }
}