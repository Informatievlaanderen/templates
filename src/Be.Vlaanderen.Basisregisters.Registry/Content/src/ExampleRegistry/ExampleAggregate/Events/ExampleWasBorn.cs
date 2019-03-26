namespace ExampleRegistry.ExampleAggregate.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("ExampleWasBorn")]
    [EventDescription("The example was born!")]
    public class ExampleWasBorn
    {
        public Guid ExampleId { get; }

        public ExampleWasBorn(
            ExampleId exampleId)
        {
            ExampleId = exampleId;
        }

        [JsonConstructor]
        private ExampleWasBorn(
            Guid exampleId)
            : this(
                new ExampleId(exampleId)) {}
    }
}
