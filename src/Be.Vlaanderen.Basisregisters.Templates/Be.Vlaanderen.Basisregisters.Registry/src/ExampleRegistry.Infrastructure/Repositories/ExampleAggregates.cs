namespace ExampleRegistry.Infrastructure.Repositories
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using ExampleAggregate;
    using SqlStreamStore;

    public class ExampleAggregates : Repository<ExampleAggregate, ExampleAggregateId>, IExampleAggregates
    {
        public ExampleAggregates(ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer)
            : base(ExampleAggregate.Factory, unitOfWork, eventStore, eventMapping, eventDeserializer) { }
    }
}
