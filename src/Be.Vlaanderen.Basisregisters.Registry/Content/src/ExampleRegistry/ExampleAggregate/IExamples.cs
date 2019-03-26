namespace ExampleRegistry.ExampleAggregate
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IExamples : IAsyncRepository<Example, ExampleId> { }
}
