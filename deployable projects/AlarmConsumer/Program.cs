using AlarmConsumer.Workers;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
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
        var a = hostContext.Configuration.GetSection("Kafka");
        
        services.Configure<ConsumerConfig>(a);
        services.AddSingleton<IConsumer<string, CoordinateMessage>>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();

            return new ConsumerBuilder<string, CoordinateMessage>(config.Value)
                .SetValueDeserializer(new JsonDeserializer<CoordinateMessage>().AsSyncOverAsync())
                .Build();
        });
        services.AddHostedService<AlarmWorker>();
    })
    .Build();

await host.RunAsync();