namespace ExampleRegistry.Infrastructure.Repositories
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using ExampleAggregate;
    using SqlStreamStore;

    public class Examples : Repository<Example, ExampleId>, IExamples
    {
        public Examples(ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer)
            : base(Example.Factory, unitOfWork, eventStore, eventMapping, eventDeserializer) { }
    }
}
