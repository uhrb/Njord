using HostInitActions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Njord.Ais.MessageProcessing;
using Njord.Ais.MessageProcessing.Sinks;
using Njord.AisStream;
using Njord.NCA;
using Njord.OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Reactive.Linq;

namespace Njord.MessageCollector
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddMetrics();
                services.Configure<AisStreamRawMessageSourceOptions>(context.Configuration.GetSection(nameof(AisStreamRawMessageSourceOptions)));
                services.Configure<NcaStreamRawMessageSourceOptions>(context.Configuration.GetSection(nameof(NcaStreamRawMessageSourceOptions)));
                services.Configure<StringCategoryMappingSinkOptions>(context.Configuration.GetSection(nameof(StringCategoryMappingSinkOptions)));
                services.AddTransient<InformationalLogMessageSink>();
                services.AddSingleton<StringCategoryMappingSink>();
                services.AddTransient<NullSink<RawAisMessage>>();
                services.AddSingleton<AisStreamMessageSourceProxy>();
                services.AddSingleton<NcaStreamRawMessageSourceProxy>();
                services.AddSingleton<IDataflowPipelineBuilder, DataflowPipelineBuilder>();
                services.AddHostedService<AisStreamRawMessageSourceService>();
                services.AddHostedService<NcaStreamRawMessageSourceService>();
                services.AddHostedService<MessageProcessor>();
                services.AddOpenTelemetry()
                    .WithMetrics(metrics =>
                    {
                        metrics
                        .AddMeter(typeof(AisStreamRawMessageSourceService).FullName!)
                        .AddMeter(typeof(StringCategoryMappingSink).FullName!)
                        .AddMeter(typeof(NcaStreamRawMessageSourceService).FullName!)
                        .AddMeter(typeof(DataflowPipelineBuilder).FullName!)
                        .AddReader(_ =>
                            new PeriodicExportingMetricReader(
                                new LogMetricExporter(
                                    _.GetRequiredService<ILogger<LogMetricExporter>>()
                                    ),
                                5000
                            )
                        );
                    });
                services.AddAsyncServiceInitialization()
                    .AddInitAction<StringCategoryMappingSink>(async service =>
                    {
                        await service.InitAsync();
                    });
            });


            using IHost host = builder.Build();

            await host.RunAsync();
        }
    }
}
