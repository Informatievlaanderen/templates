namespace ExampleRegistry.ExampleAggregate.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("ExampleHappened")]
    [EventDescription("The example happened, whatever it was.")]
    public class ExampleHappened
    {
        public Guid ExampleId { get; }
        public string Name { get; }
        public Language Language { get; }

        public ExampleHappened(
            ExampleId exampleId,
            ExampleName exampleName)
        {
            ExampleId = exampleId;

            Language = exampleName.Language;
            Name = exampleName.Name;
        }

        [JsonConstructor]
        private ExampleHappened(
            Guid exampleId,
            string name,
            Language language)
            : this(
                new ExampleId(exampleId),
                new ExampleName(name, language)) {}
    }
}
