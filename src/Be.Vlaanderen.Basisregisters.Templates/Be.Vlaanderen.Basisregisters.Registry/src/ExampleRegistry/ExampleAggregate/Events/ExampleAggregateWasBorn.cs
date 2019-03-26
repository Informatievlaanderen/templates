namespace ExampleRegistry.ExampleAggregate.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("ExampleAggregateWasBorn")]
    [EventDescription("The ExampleAggregate was born!")]
    public class ExampleAggregateWasBorn
    {
        public Guid ExampleAggregateId { get; }

        public ExampleAggregateWasBorn(
            ExampleAggregateId exampleAggregateId)
        {
            ExampleAggregateId = exampleAggregateId;
        }

        [JsonConstructor]
        private ExampleAggregateWasBorn(
            Guid exampleAggregateId)
            : this(
                new ExampleAggregateId(exampleAggregateId)) {}
    }
}
