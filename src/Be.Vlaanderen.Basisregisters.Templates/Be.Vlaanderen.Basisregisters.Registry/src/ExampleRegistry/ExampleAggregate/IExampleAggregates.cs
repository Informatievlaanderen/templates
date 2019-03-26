namespace ExampleRegistry.ExampleAggregate
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IExampleAggregates : IAsyncRepository<ExampleAggregate, ExampleAggregateId> { }
}
