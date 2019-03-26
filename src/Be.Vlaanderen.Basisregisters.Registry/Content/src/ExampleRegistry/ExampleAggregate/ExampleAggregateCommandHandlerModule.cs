namespace ExampleRegistry.ExampleAggregate
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Commands;
    using SqlStreamStore;

    public sealed class ExampleAggregateCommandHandlerModule : CommandHandlerModule
    {
        public ExampleAggregateCommandHandlerModule(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Func<IExampleAggregates> getExampleAggregates)
        {
            For<NameExampleAggregate>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var exampleAggregates = getExampleAggregates();

                    var exampleAggregateId = message.Command.ExampleAggregateId;
                    var possibleExampleAggregate = await exampleAggregates.GetOptionalAsync(exampleAggregateId, ct);

                    if (!possibleExampleAggregate.HasValue)
                    {
                        possibleExampleAggregate = new Optional<ExampleAggregate>(ExampleAggregate.Register(exampleAggregateId));
                        exampleAggregates.Add(exampleAggregateId, possibleExampleAggregate.Value);
                    }

                    var exampleAggregate = possibleExampleAggregate.Value;

                    exampleAggregate.NameExampleAggregate(message.Command.ExampleAggregateName);
                });
        }
    }
}
