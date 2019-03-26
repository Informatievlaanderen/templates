namespace ExampleRegistry.ExampleAggregate
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Events;

    public partial class ExampleAggregate : AggregateRootEntity
    {
        public static readonly Func<ExampleAggregate> Factory = () => new ExampleAggregate();

        public static ExampleAggregate Register(ExampleAggregateId exampleAggregateId)
        {
            var exampleAggregate = Factory();
            exampleAggregate.ApplyChange(new ExampleAggregateWasBorn(exampleAggregateId));
            return exampleAggregate;
        }

        public void NameExampleAggregate(ExampleAggregateName exampleAggregateName)
        {
            ApplyChange(new ExampleAggregateWasNamed(_exampleAggregateId, exampleAggregateName));
        }
    }
}
