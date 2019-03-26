namespace ExampleRegistry.Projections.Api.ExampleAggregateList
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using ExampleAggregate.Events;

    public class ExampleAggregateListProjections : ConnectedProjection<ApiProjectionsContext>
    {
        public ExampleAggregateListProjections()
        {
            When<Envelope<ExampleAggregateWasBorn>>(async (context, message, ct) =>
            {
                await context
                    .ExampleAggregateList
                    .AddAsync(
                        new ExampleAggregateList
                        {
                            Id = message.Message.ExampleAggregateId,
                        }, ct);
            });

            When<Envelope<ExampleAggregateWasNamed>>(async (context, message, ct) =>
                await context.FindAndUpdateExampleAggregateList(
                    message.Message.ExampleAggregateId,
                    exampleAggregate =>
                    {
                        // Contrived example, if the name is Dutch, we always take it.
                        // Otherwise we only write it if we dont have any yet.
                        if (message.Message.Language == Language.Dutch)
                            exampleAggregate.Name = message.Message.Name;

                        if (string.IsNullOrWhiteSpace(exampleAggregate.Name))
                            exampleAggregate.Name = message.Message.Name;
                    },
                    ct));
        }
    }
}
