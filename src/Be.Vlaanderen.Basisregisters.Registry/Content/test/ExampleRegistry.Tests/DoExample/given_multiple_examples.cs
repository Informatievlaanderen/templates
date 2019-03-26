namespace ExampleRegistry.Tests.DoExample
{
    using System;
    using Xunit;
    using Xunit.Abstractions;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using ExampleAggregate.Commands;
    using ExampleAggregate.Events;

    public class given_multiple_examples : ExampleRegistryTest
    {
        public given_multiple_examples(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Theory, DefaultData]
        public void then_example_should_be_born_and_had_happened_multiple_times(
            NameExampleAggregate nameExampleAggregateCommand)
        {
            var fixtureLanguage = nameExampleAggregateCommand.ExampleAggregateName.Language;

            Assert(new Scenario()
                .Given(nameExampleAggregateCommand.ExampleAggregateId,
                    new ExampleAggregateWasBorn(nameExampleAggregateCommand.ExampleAggregateId),
                    new ExampleAggregateWasNamed(nameExampleAggregateCommand.ExampleAggregateId, new ExampleAggregateName("Bla", fixtureLanguage == Language.Dutch ? Language.English : Language.Dutch)))
                .When(nameExampleAggregateCommand)
                .Then(nameExampleAggregateCommand.ExampleAggregateId,
                    new ExampleAggregateWasNamed(nameExampleAggregateCommand.ExampleAggregateId, nameExampleAggregateCommand.ExampleAggregateName)));
        }
    }
}
