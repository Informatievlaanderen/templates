namespace ExampleRegistry.Projections.Api.Tests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector.Testing;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public static class ScenarioExtensions
    {
        public static ConnectedProjectionScenario<ApiProjectionsContext> Scenario(this ConnectedProjection<ApiProjectionsContext> projection) =>
            new ConnectedProjectionScenario<ApiProjectionsContext>(Resolve.WhenEqualToHandlerMessageType(projection.Handlers));

        public static Task Expect(
            this ConnectedProjectionScenario<ApiProjectionsContext> scenario,
            ITestOutputHelper testOutputHelper,
            IEnumerable<object> records) => scenario.Expect(testOutputHelper, records.ToArray());

        public static async Task Expect(
            this ConnectedProjectionScenario<ApiProjectionsContext> scenario,
            ITestOutputHelper testOutputHelper,
            params object[] records)
        {
            var database = Guid.NewGuid().ToString("N");

            var specification = scenario.Verify(async context =>
            {
                var comparisonConfig = new ComparisonConfig { MaxDifferences = 100 };
                var comparer = new CompareLogic(comparisonConfig);
                var actualRecords = await context.AllRecords();
                var result = comparer.Compare(
                    actualRecords,
                    records);

                return result.AreEqual
                    ? VerificationResult.Pass()
                    : VerificationResult.Fail(result.CreateDifferenceMessage(actualRecords, records));
            });

            using (var context = CreateContextFor(database, testOutputHelper))
            {
                var projector = new ConnectedProjector<ApiProjectionsContext>(specification.Resolver);

                foreach (var message in specification.Messages)
                {
                    var envelope = new Envelope(message, new Dictionary<string, object>()).ToGenericEnvelope();
                    await projector.ProjectAsync(context, envelope);
                }

                await context.SaveChangesAsync();

                var result = await specification.Verification(context, CancellationToken.None);

                if (result.Failed)
                    throw specification.CreateFailedScenarioExceptionFor(result);
            }
        }

        private static async Task<object[]> AllRecords(this ApiProjectionsContext context)
        {
            var records = new List<object>();

#if (!ExcludeExampleAggregate)
            records.AddRange(await context.ExampleAggregateDetails.ToArrayAsync());
            records.AddRange(await context.ExampleAggregateList.ToArrayAsync());
#endif

            return records.ToArray();
        }

        private static ApiProjectionsContext CreateContextFor(string database, ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));

            var options = new DbContextOptionsBuilder<ApiProjectionsContext>()
                .UseInMemoryDatabase(database)
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory)
                .Options;

            return new ApiProjectionsContext(options);
        }

        private static XunitException CreateFailedScenarioExceptionFor(
            this ConnectedProjectionTestSpecification<ApiProjectionsContext> specification,
            VerificationResult result)
        {
            var title = string.Empty;

            var exceptionMessage = new StringBuilder()
                .AppendLine(title)
                .AppendTitleBlock("Given", specification.Messages, Formatters.NamedJsonMessage)
                .Append(result.Message);

            return new XunitException(exceptionMessage.ToString());
        }
    }
}
