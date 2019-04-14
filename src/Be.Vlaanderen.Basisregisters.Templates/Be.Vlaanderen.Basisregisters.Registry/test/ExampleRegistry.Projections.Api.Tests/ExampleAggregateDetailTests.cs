namespace ExampleRegistry.Projections.Api.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using ExampleAggregate.Events;
    using ExampleAggregateDetail;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;
    using ExampleRegistry.Tests.Infrastructure;

    public class ExampleAggregateDetailTests : ProjectionsTest
    {
        public Fixture Fixture { get; }

        public ExampleAggregateDetailTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            Fixture = new Fixture();
            Fixture.CustomizeExampleAggregateName();
        }

        [Fact]
        public Task when_example_aggregates_are_born()
        {
            var data = Fixture
                .CreateMany<ExampleAggregateWasBorn>(new Random().Next(1, 100))
                .Select(@event =>
                {
                    var expectedRecord = new ExampleAggregateDetail
                    {
                        Id = @event.ExampleAggregateId
                    };

                    return new
                    {
                        ExampleAggregateWasBorn = @event,
                        ExpectedRecord = expectedRecord
                    };
                }).ToList();

            return new ExampleAggregateDetailProjections()
                .Scenario()
                .Given(data.Select(d => d.ExampleAggregateWasBorn))
                .Expect(TestOutputHelper, data.Select(d => d.ExpectedRecord));
        }

        [Fact]
        public Task when_example_aggregates_are_named()
        {
            var previousEvents = Fixture
                .CreateMany<ExampleAggregateWasBorn>(new Random().Next(1, 100))
                .ToList();

            var events = previousEvents
                .Select(x =>
                    new ExampleAggregateWasNamed(
                        new ExampleAggregateId(x.ExampleAggregateId),
                        Fixture.Create<ExampleAggregateName>()))
                .ToList();

            var expected = events
                .Select(x =>
                {
                    var record = new ExampleAggregateDetail { Id = x.ExampleAggregateId };

                    switch (x.Language)
                    {
                        case Language.Dutch:
                            record.NameDutch = x.Name;
                            break;

                        case Language.French:
                            record.NameFrench = x.Name;
                            break;

                        case Language.German:
                            record.NameGerman = x.Name;
                            break;

                        case Language.English:
                            record.NameEnglish = x.Name;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return record;
                })
                .ToList();

            return new ExampleAggregateDetailProjections()
                .Scenario()
                .Given(previousEvents.Cast<object>().Union(events))
                .Expect(TestOutputHelper, expected);
        }
    }
}
