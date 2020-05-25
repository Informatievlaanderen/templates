namespace ExampleRegistry.Api.Tests.Infrastructure
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit.Abstractions;

    public class ApiTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ICommandHandlerResolver _stubResolver = new StubCommandHandlerResolver();

        public HttpClient Client { get; }

        public ApiTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var server = CreateServer();

            Client = CreateClient(server);
        }

        protected TestServer CreateServer() =>
            new TestServer(
                new WebHostBuilder()
                    .ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddProvider(new XunitLoggerProvider(_testOutputHelper));
                    })
                    .ConfigureServices(services => services.AddSingleton(_stubResolver))
                    .UseStartup<TestStartup>());

        protected HttpClient CreateClient(TestServer server)
        {
            var client = server.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            return client;
        }

        protected async Task<List<object>> Post<T>(string endpoint, T request)
        {
            await Client
                .PostAsync(
                    endpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json));

            return ((StubCommandHandlerResolver)_stubResolver).ReceivedCommands;
        }

        protected async Task<List<object>> Put<T>(string endpoint, T request)
        {
            await Client
                .PutAsync(
                    endpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json));

            return ((StubCommandHandlerResolver)_stubResolver).ReceivedCommands;
        }
    }
}
