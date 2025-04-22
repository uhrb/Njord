using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Memory;
using Njord.Ais.Interfaces;
using Njord.Ais.MessageProcessing;
using Njord.Ais.MessageProcessing.Sinks;
using Njord.AisStream;
using Njord.OpenTelemetry;
using Njord.Server.HostedServices;
using Njord.Server.Hubs;
using Njord.Server.Intstrumentation;
using Njord.Server.Services;
using OpenTelemetry.Metrics;
using Orleans.Serialization;
using Orleans.Runtime.Hosting;
using Orleans.Providers;
using Njord.Server.Grains.Instrumentation;
using Njord.NCA;

namespace Njord.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<AisStreamRawMessageSourceOptions>(builder.Configuration.GetSection(nameof(AisStreamRawMessageSourceOptions)));
            builder.Services.Configure<NcaStreamRawMessageSourceOptions>(builder.Configuration.GetSection(nameof(NcaStreamRawMessageSourceOptions)));
            builder.Services.AddSingleton<IMessageBroadcaster, MessageBroadcaster>();
            builder.Services.AddTransient<InformationalLogMessageSink>();
            builder.Services.AddTransient<NullSink<RawAisMessage>>();
            builder.Services.AddTransient<NullSink<IMessageId>>();
            builder.Services.AddTransient<OrleansSink>();
            builder.Services.AddTransient<AisStreamMessageTransformer>();
            builder.Services.AddSingleton<AisStreamMessageSourceProxy>();
            builder.Services.AddSingleton<NcaStreamRawMessageSourceProxy>();
            builder.Services.AddTransient<IDataflowPipelineBuilder, DataflowPipelineBuilder>();
            builder.Services.AddHostedService<MessageProcessingService>();
            builder.Services.AddHostedService<NcaStreamRawMessageSourceService>();
            builder.Services.AddHostedService<AisStreamRawMessageSourceService>();
            builder.Services.AddTransient<ServerHealthCheck>();
            builder.Services.AddSingleton<IGrainAccessor, GrainAccessor>();
            builder.Services.AddMemoryCache();
            builder.Services.AddHealthChecks()
                .AddCheck<ServerHealthCheck>("metrics");
            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddMeter("Microsoft.Orleans")
                        .AddMeter(typeof(AisStreamRawMessageSourceService).FullName!)
                        .AddMeter(typeof(DataflowPipelineBuilder).FullName!)
                        .AddMeter(typeof(NcaStreamRawMessageSourceService).FullName!)
                        .AddReader(_ =>
                            new PeriodicExportingMetricReader(
                                new LogMetricExporter(
                                        _.GetRequiredService<ILogger<LogMetricExporter>>()
                                    )
                            )
                        )
                        .AddReader(_ =>
                            new PeriodicExportingMetricReader(
                                new MemoryCacheExporter(
                                        _.GetRequiredService<IMemoryCache>(), "metrics"
                                    )
                            )
                        );
                });
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSignalR();
            builder.UseOrleans(sb =>
                   {
                       sb.UseDashboard(cfg =>
                       {
                       });
                       sb.UseLocalhostClustering();
                       sb.Services.AddSingleton<InMemoryGrainStorage>();
                       sb.Services.AddSingleton<IGrainStorageWithQueries>(_ => _.GetRequiredService<InMemoryGrainStorage>());
                       sb.Services.AddGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME, (sp, name) => sp.GetRequiredService<InMemoryGrainStorage>());
                       sb.Services.AddSerializer(serde =>
                       {
                           serde.AddJsonSerializer(
                               isSupported: type => true
                               );
                       });
                   });
            
            var app = builder.Build();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(report);
                }
            });
            app.MapOpenApi();
            app.MapHub<MapUpdatesHub>("/map-updates");
            app.MapControllers();
            app.Run();
        }
    }
}
