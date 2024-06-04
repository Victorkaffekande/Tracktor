using AlarmConsumer.Models;
using AlarmConsumer.Workers;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedModels;

DotNetEnv.Env.Load("../../../.env");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var kafkaConfiguration = hostContext.Configuration.GetSection("Kafka");
        
        services.Configure<ProducerConfig>(kafkaConfiguration);
        services.Configure<SchemaRegistryConfig>(hostContext.Configuration.GetSection("SchemaRegistry"));
        services.Configure<ConsumerConfig>(kafkaConfiguration);
        
        services.AddSingleton<IConsumer<string, LocationMessage>>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();

            return new ConsumerBuilder<string, LocationMessage>(config.Value)
                .SetValueDeserializer(new JsonDeserializer<LocationMessage>().AsSyncOverAsync())
                .Build();
        });
        
        services.AddSingleton<ISchemaRegistryClient>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<SchemaRegistryConfig>>();

            return new CachedSchemaRegistryClient(config.Value);
        });
        
        services.AddSingleton<IProducer<String, AlarmMessage>>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ProducerConfig>>();
            var schema = sp.GetRequiredService<ISchemaRegistryClient>();

            return new ProducerBuilder<String, AlarmMessage>(config.Value)
                .SetValueSerializer(new JsonSerializer<AlarmMessage>(schema))
                .Build();
        });
        
        services.AddHostedService<AlarmWorker>();
    })
    .Build();

await host.RunAsync();