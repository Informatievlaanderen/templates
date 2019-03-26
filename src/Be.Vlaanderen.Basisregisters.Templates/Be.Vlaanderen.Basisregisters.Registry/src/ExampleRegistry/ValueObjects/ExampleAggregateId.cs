namespace ExampleRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Exceptions;
    using Newtonsoft.Json;

    public class ExampleAggregateId : GuidValueObject<ExampleAggregateId>
    {
        public ExampleAggregateId([JsonProperty("value")] Guid exampleAggregateId) : base(exampleAggregateId)
        {
            if (exampleAggregateId == Guid.Empty)
                throw new NoExampleAggregateIdException();
        }
    }
}
