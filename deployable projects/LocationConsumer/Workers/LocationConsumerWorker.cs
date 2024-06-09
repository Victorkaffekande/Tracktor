using Confluent.Kafka;  
using LocationConsumer.Repo;  
using Microsoft.Extensions.Hosting;  
using SharedModels;  
  
namespace LocationConsumer.Workers  
{  
    public class LocationConsumerWorker : BackgroundService  
    {  
        private const string LocationsTopicName = "GPS_Locations";  
        private readonly IConsumer<string, LocationMessage> _consumer;  
        private readonly ILocationRepo _repo;  
        private const int BatchSize = 100;  
        private const int PollTimeoutMs = 1000;  
        private const int IndividualPollTimeoutMs = 150;   
  
        public LocationConsumerWorker(IConsumer<string, LocationMessage> consumer, ILocationRepo repo)  
        {  
            _consumer = consumer;  
            _repo = repo;  
        }  
  
        private async Task HandleMessages(IEnumerable<Message<string, LocationMessage>> messages)  
        {  
            var locations = new List<Location>();  
            
            foreach (var message in messages)  
            {  
                locations.Add(new Location()  
                {  
                    Timestamp = message.Timestamp.UtcDateTime,  
                    Latitude = message.Value.Latitude,  
                    Longitude = message.Value.Longitude,  
                    VehicleId = message.Value.VehicleId,  
                    FleetId = message.Value.FleetId  
                });  
            }  
            if (locations.Count > 0)
            {
                await _repo.BulkInsert(locations);
                Console.WriteLine($"Processed batch of {locations.Count} locations.");  
            }  
        }  
  
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)  
        {  
            _consumer.Subscribe(LocationsTopicName);  
            try  
            {  
                while (!stoppingToken.IsCancellationRequested)  
                {  
                    var messages = new List<Message<string, LocationMessage>>();  
                    var pollStartTime = DateTime.UtcNow;  
                    
                    while (messages.Count < BatchSize && (DateTime.UtcNow - pollStartTime).TotalMilliseconds < PollTimeoutMs)  
                    {  
                        var result = _consumer.Consume(TimeSpan.FromMilliseconds(IndividualPollTimeoutMs));
                        if (result != null)  
                        {  
                            messages.Add(result.Message);
                        }  
                    }  

                    if (messages.Count > 0)  
                    {  
                        await HandleMessages(messages);  
                    }  
                }  
            }  
            finally  
            {  
                _consumer.Close();  
            }  
        }  
    }  
}  
