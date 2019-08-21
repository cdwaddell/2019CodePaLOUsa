using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Xunit;

namespace MicroServicesDemo.Tests
{
    public static class HttpClientNames
    {
        public const string LocalHost = "LocalHost";
    }

    public class TestingCSharpFrameworks
    {
        [Fact]
        public async Task PollyTest()
        {
    //public class Startup
    //{
        //...
        var services = new ServiceCollection();
        //public void ConfigureServices(IServiceCollection services)
        //{
        //...
            //Bulkhead policies apply to all requests, you should only
            //use one from all ttpClients
            var bulkhead = Policy
                .Bulkhead(
                    maxParallelization: 10,
                    maxQueuingActions: 2
                );

            var breaker = Policy
                .Handle<HttpRequestException>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: 2, 
                    durationOfBreak: TimeSpan.FromMinutes(1)
                );

            var advancedBreaker = Policy
                .Handle<HttpRequestException>()
                .AdvancedCircuitBreaker(
                    failureThreshold: .5,
                    samplingDuration: TimeSpan.FromSeconds(2),
                    minimumThroughput: 10,
                    TimeSpan.FromSeconds(1));

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner call times out
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                });

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(1); // Timeout for an individual try

            services.AddHttpClient(HttpClientNames.LocalHost, client =>
                {
                    client.BaseAddress = new Uri("http://localhost:5337/");
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                    client.Timeout = TimeSpan.FromSeconds(60); // Overall timeout across all tries
                })
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy); // We place the timeoutPolicy inside the retryPolicy, to make it time out each try.
        //...
        //}
    //}
            var provider = services.BuildServiceProvider();

            await Enumerable.Range(1, 10).ForEachAsync(10, async i =>
            {
                var clientFactory = provider.GetService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient(HttpClientNames.LocalHost);

                await httpClient.GetAsync($"api/error/500/{i}");
            });
        }
    }

    public static class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body) 
        { 
            return Task.WhenAll( 
                from partition in Partitioner.Create(source).GetPartitions(dop) 
                select Task.Run(async delegate { 
                    using (partition) 
                        while (partition.MoveNext()) 
                            await body(partition.Current); 
                })); 
        }
    }
}
