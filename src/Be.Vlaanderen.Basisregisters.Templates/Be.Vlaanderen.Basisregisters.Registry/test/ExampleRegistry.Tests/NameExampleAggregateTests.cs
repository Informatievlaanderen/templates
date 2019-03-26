namespace ExampleRegistry.Tests
{
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using ExampleAggregate.Commands;
    using ExampleAggregate.Events;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class NameExampleAggregateTests : ExampleRegistryTest
    {
        public Fixture Fixture { get; }

        public NameExampleAggregateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture = new Fixture();
            Fixture.CustomizeExampleAggregateName();
        }

        [Fact]
        public void should_have_been_created()
        {
            var command = Fixture.Create<NameExampleAggregate>();

            Assert(new Scenario()
                .GivenNone()
                .When(command)
                .Then(command.ExampleAggregateId,
                    new ExampleAggregateWasBorn(command.ExampleAggregateId),
                    new ExampleAggregateWasNamed(command.ExampleAggregateId, command.ExampleAggregateName)));
        }

        [Fact]
        public void should_be_named_twice()
        {
            var id = Fixture.Create<ExampleAggregateId>();
            var name = Fixture.Create<ExampleAggregateName>();
            var name2 = Fixture.Create<ExampleAggregateName>();
            var command = new NameExampleAggregate(id, name2);

            Assert(new Scenario()
                .Given(id,
                    new ExampleAggregateWasBorn(id),
                    new ExampleAggregateWasNamed(id, name))
                .When(command)
                .Then(command.ExampleAggregateId,
                    new ExampleAggregateWasNamed(command.ExampleAggregateId, command.ExampleAggregateName)));
        }
    }
}
