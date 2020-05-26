namespace ExampleRegistry.ExampleAggregate.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("ExampleAggregateWasNamed")]
    [EventDescription("The example aggregate was named in a specific language.")]
    public class ExampleAggregateWasNamed
    {
        public Guid ExampleAggregateId { get; }

        public string Name { get; }

        public Language Language { get; }

        public ExampleAggregateWasNamed(
            ExampleAggregateId exampleAggregateId,
            ExampleAggregateName exampleAggregateName)
        {
            ExampleAggregateId = exampleAggregateId;

            Language = exampleAggregateName.Language;
            Name = exampleAggregateName.Name;
        }

        [JsonConstructor]
        private ExampleAggregateWasNamed(
            [JsonProperty("exampleAggregateId")] Guid exampleAggregateId,
            [JsonProperty("name")] string name,
            [JsonProperty("language")] Language language)
            : this(
                new ExampleAggregateId(exampleAggregateId),
                new ExampleAggregateName(name, language)) { }
    }
}
