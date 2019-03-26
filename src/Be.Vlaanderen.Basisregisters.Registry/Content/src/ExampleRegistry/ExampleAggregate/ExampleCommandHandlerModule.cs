namespace ExampleRegistry.ExampleAggregate
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Commands;
    using SqlStreamStore;

    public sealed class SimpleExampleCommandHandlerModule : CommandHandlerModule
    {
        public SimpleExampleCommandHandlerModule()
        {
            For<DoSimpleExample>()
                .Handle(message =>
                {
                    Console.WriteLine($"A simple example arrived, saying {message.Command.Name.Name} in {message.Command.Name.Language}!");
                });
        }
    }

    public sealed class ExampleCommandHandlerModule : CommandHandlerModule
    {
        public ExampleCommandHandlerModule(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Func<IExamples> getExamples)
        {
            For<DoExample>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var examples = getExamples();

                    var exampleId = message.Command.ExampleId;
                    var possibleExample = await examples.GetOptionalAsync(exampleId, ct);

                    if (!possibleExample.HasValue)
                    {
                        possibleExample = new Optional<Example>(Example.Register(exampleId));
                        examples.Add(exampleId, possibleExample.Value);
                    }

                    var example = possibleExample.Value;

                    example.DoExample(message.Command.Name);
                });
        }
    }
}
