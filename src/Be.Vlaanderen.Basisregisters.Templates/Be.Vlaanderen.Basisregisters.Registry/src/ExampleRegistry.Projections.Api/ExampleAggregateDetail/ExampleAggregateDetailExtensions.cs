namespace ExampleRegistry.Projections.Api.ExampleAggregateDetail
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;

    public static class ExampleAggregateDetailExtensions
    {
        public static async Task<ExampleAggregateDetail> FindAndUpdateExampleAggregateDetail(
            this ApiProjectionsContext context,
            Guid exampleAggregateId,
            Action<ExampleAggregateDetail> updateFunc,
            CancellationToken ct)
        {
            var exampleAggregate = await context
                .ExampleAggregateDetails
                .FindAsync(exampleAggregateId, cancellationToken: ct);

            if (exampleAggregate == null)
                throw DatabaseItemNotFound(exampleAggregateId);

            updateFunc(exampleAggregate);

            return exampleAggregate;
        }

        private static ProjectionItemNotFoundException<ExampleAggregateDetailProjections> DatabaseItemNotFound(Guid exampleAggregateId)
            => new ProjectionItemNotFoundException<ExampleAggregateDetailProjections>(exampleAggregateId.ToString("D"));
    }
}
