namespace ExampleRegistry.Projections.Api.ExampleAggregateDetail
{
    using System;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using ExampleAggregate.Events;

    public class ExampleAggregateDetailProjections : ConnectedProjection<ApiProjectionsContext>
    {
        public ExampleAggregateDetailProjections()
        {
            When<Envelope<ExampleAggregateWasBorn>>(async (context, message, ct) =>
            {
                await context
                    .ExampleAggregateDetails
                    .AddAsync(
                        new ExampleAggregateDetail
                        {
                            Id = message.Message.ExampleAggregateId,
                        }, ct);
            });

            When<Envelope<ExampleAggregateWasNamed>>(async (context, message, ct) =>
                await context.FindAndUpdateExampleAggregateDetail(
                    message.Message.ExampleAggregateId,
                    exampleAggregate =>
                    {
                        switch (message.Message.Language)
                        {
                            case Language.Dutch:
                                exampleAggregate.NameDutch = message.Message.Name;
                                break;

                            case Language.French:
                                exampleAggregate.NameFrench = message.Message.Name;
                                break;

                            case Language.German:
                                exampleAggregate.NameGerman = message.Message.Name;
                                break;

                            case Language.English:
                                exampleAggregate.NameEnglish = message.Message.Name;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(message.Message.Language));
                        }
                    },
                    ct));
        }
    }
}
