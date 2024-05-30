using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Options;
using Confluent.Kafka.SyncOverAsync;
using LocationConsumer.Repo;
using LocationConsumer.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedModels;

DotNetEnv.Env.Load("../../../.env");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(CassandraSessionFactory.CreateCassandraService().Start());
        services.AddSingleton<ILocationRepo, LocationRepo>();
        services.Configure<ConsumerConfig>(hostContext.Configuration.GetSection("Kafka")); 
        services.AddSingleton<IConsumer<String, LocationMessage>>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();

            return new ConsumerBuilder<String, LocationMessage>(config.Value)
                .SetValueDeserializer(new JsonDeserializer<LocationMessage>().AsSyncOverAsync())
                .Build();
        });
        services.AddHostedService<LocationConsumerWorker>();
    })
    .Build();

await host.RunAsync();