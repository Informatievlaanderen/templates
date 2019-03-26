namespace ExampleRegistry.Tests.DoExample
{
    using Xunit;
    using Xunit.Abstractions;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using ExampleAggregate.Commands;
    using ExampleAggregate.Events;

    public class given_one_example : ExampleRegistryTest
    {
        public given_one_example(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory, DefaultData]
        public void then_example_should_have_been_born_and_had_happened(
            NameExampleAggregate nameExampleAggregateCommand)
        {
            Assert(new Scenario()
                .GivenNone()
                .When(nameExampleAggregateCommand)
                .Then(nameExampleAggregateCommand.ExampleAggregateId,
                    new ExampleAggregateWasBorn(nameExampleAggregateCommand.ExampleAggregateId),
                    new ExampleAggregateWasNamed(nameExampleAggregateCommand.ExampleAggregateId, nameExampleAggregateCommand.ExampleAggregateName)));
        }
    }
}
