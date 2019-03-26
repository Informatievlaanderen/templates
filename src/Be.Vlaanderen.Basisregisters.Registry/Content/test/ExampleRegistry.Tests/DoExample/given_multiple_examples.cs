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
            DoExample doExampleCommand)
        {
            var fixtureLanguage = doExampleCommand.Name.Language;

            Assert(new Scenario()
                .Given(doExampleCommand.ExampleId,
                    new ExampleWasBorn(doExampleCommand.ExampleId),
                    new ExampleHappened(doExampleCommand.ExampleId, new ExampleName("Bla", fixtureLanguage == Language.Dutch ? Language.English : Language.Dutch)))
                .When(doExampleCommand)
                .Then(doExampleCommand.ExampleId,
                    new ExampleHappened(doExampleCommand.ExampleId, doExampleCommand.Name)));
        }
    }
}
