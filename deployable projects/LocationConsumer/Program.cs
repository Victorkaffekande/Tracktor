using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Options;
using Confluent.Kafka.SyncOverAsync;
using LocationConsumer.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedModels;

DotNetEnv.Env.Load();

Console.WriteLine($"BootstrapServers: {Environment.GetEnvironmentVariable("Consumer__BootstrapServers")}");  
Console.WriteLine($"SaslUsername: {Environment.GetEnvironmentVariable("Consumer__SaslUsername")}");  
Console.WriteLine($"SaslPassword: {Environment.GetEnvironmentVariable("Consumer__SaslPassword")}");  

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<ConsumerConfig>(hostContext.Configuration.GetSection("Consumer")); 
        services.AddSingleton<IConsumer<String, CoordinateMessage>>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();

            return new ConsumerBuilder<String, CoordinateMessage>(config.Value)
                .SetValueDeserializer(new JsonDeserializer<CoordinateMessage>().AsSyncOverAsync())
                .Build();
        });
        services.AddHostedService<LocationConsumerWorker>();
    })
    .Build();

await host.RunAsync();