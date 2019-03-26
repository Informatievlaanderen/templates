namespace ExampleRegistry.ExampleAggregate
{
    using System.Collections.Generic;
    using Events;

    public partial class ExampleAggregate
    {
        private ExampleAggregateId _exampleAggregateId;

        private readonly Dictionary<Language, ExampleAggregateName> _names
            = new Dictionary<Language, ExampleAggregateName>();

        private ExampleAggregate()
        {
            Register<ExampleAggregateWasBorn>(When);
            Register<ExampleAggregateWasNamed>(When);
        }

        private void When(ExampleAggregateWasBorn @event)
        {
            _exampleAggregateId = new ExampleAggregateId(@event.ExampleAggregateId);
        }

        private void When(ExampleAggregateWasNamed @event)
        {
            _names[@event.Language] = new ExampleAggregateName(@event.Name, @event.Language);
        }
    }
}
