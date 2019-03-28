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

    public class ExampleAggregateDetailTests : ProjectionsTest
    {
        private readonly Fixture _fixture;

        public ExampleAggregateDetailTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _fixture = new Fixture();
        }

        [Fact]
        public Task when_example_aggregates_are_born()
        {
            var data = _fixture
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
            var existingRecords = _fixture
                .CreateMany<Guid>(new Random().Next(1, 100))
                .Select(x => new ExampleAggregateDetail { Id = x })
                .ToList();

            var previousEvents = existingRecords
                .Select(x =>
                    new ExampleAggregateWasBorn(
                        new ExampleAggregateId(x.Id)))
                .ToList();

            var events = existingRecords
                .Select(x =>
                    new ExampleAggregateWasNamed(
                        new ExampleAggregateId(x.Id),
                        _fixture.Create<ExampleAggregateName>()))
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
