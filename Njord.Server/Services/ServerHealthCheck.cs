using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Njord.Server.Services
{
    public class ServerHealthCheck : IHealthCheck
    {
        private readonly ILogger<ServerHealthCheck> _logger;
        private readonly IMemoryCache _memoryCache;

        public ServerHealthCheck(ILogger<ServerHealthCheck> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HealthCheck invoked");
            var state = _memoryCache.Get<object>("metrics");
            return Task.FromResult(HealthCheckResult.Healthy("Running", new Dictionary<string, object>
            {
                { "Metrics" , state },
            }));
        }
    }
}
