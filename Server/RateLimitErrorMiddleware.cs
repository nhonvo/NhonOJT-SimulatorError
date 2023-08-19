using System.Collections.Concurrent;
using System.Threading.RateLimiting;

namespace Server
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _requestLocks;
        private readonly int _permitLimit;
        private readonly TimeSpan _window;
        private readonly QueueProcessingOrder _queueProcessingOrder;
        private readonly int _queueLimit;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
            _requestLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
            _permitLimit = 3;
            _window = TimeSpan.FromSeconds(5);
            _queueProcessingOrder = QueueProcessingOrder.OldestFirst;
            _queueLimit = 3;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var requestPath = context.Request.Path.ToString();

            var requestLock = _requestLocks.GetOrAdd($"{ipAddress}:{requestPath}", _ => new SemaphoreSlim(1));

            await requestLock.WaitAsync();

            try
            {
                // Perform rate limiting logic here
                // e.g., check if the number of requests from the IP address within the window has exceeded the limit
                var requestCount = GetRequestCount(ipAddress, requestPath);

                if (requestCount >= _permitLimit)
                {
                    // If the limit is exceeded, return a rate-limiting response
                    context.Response.StatusCode = 429; // 429 Too Many Requests
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }

                // If the limit is not exceeded, proceed with the request
                await _next(context);
            }
            finally
            {
                requestLock.Release();
            }
        }

        private int GetRequestCount(string ipAddress, string requestPath)
        {
            // Implement your own logic to track and count requests from the given IP address and request path within the specified window.
            // You can use a database, caching, or any other data store to store the request count.
            // For the sake of simplicity, I'll just use a dummy implementation here.

            // For a real-world scenario, you should handle race conditions and potential exceptions while accessing the data store.
            // For simplicity, I'm skipping those checks here.

            // In a real-world scenario, you should consider using a distributed cache or database for scalability.

            // Dummy implementation to track request count (not suitable for production use)
            // You should replace this with a proper implementation.
            // For example, using a distributed cache like Redis or a database to store request counts.
            var key = $"{ipAddress}:{requestPath}";
            requestCounts.TryGetValue(key, out var count);
            count++;
            requestCounts[key] = count;
            return count;
        }

        // Dummy dictionary to store request counts (not suitable for production use)
        // You should replace this with a proper data store.
        private static readonly Dictionary<string, int> requestCounts = new Dictionary<string, int>();

    }

}