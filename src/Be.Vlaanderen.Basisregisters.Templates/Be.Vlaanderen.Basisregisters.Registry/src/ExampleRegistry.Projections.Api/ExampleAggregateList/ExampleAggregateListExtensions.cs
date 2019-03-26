namespace ExampleRegistry.Projections.Api.ExampleAggregateList
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;

    public static class ExampleAggregateListExtensions
    {
        public static async Task<ExampleAggregateList> FindAndUpdateExampleAggregateList(
            this ApiProjectionsContext context,
            Guid exampleAggregateId,
            Action<ExampleAggregateList> updateFunc,
            CancellationToken ct)
        {
            var exampleAggregate = await context
                .ExampleAggregateList
                .FindAsync(exampleAggregateId, cancellationToken: ct);

            if (exampleAggregate == null)
                throw DatabaseItemNotFound(exampleAggregateId);

            updateFunc(exampleAggregate);

            return exampleAggregate;
        }

        private static ProjectionItemNotFoundException<ExampleAggregateListProjections> DatabaseItemNotFound(Guid exampleAggregateId)
            => new ProjectionItemNotFoundException<ExampleAggregateListProjections>(exampleAggregateId.ToString("D"));
    }
}
