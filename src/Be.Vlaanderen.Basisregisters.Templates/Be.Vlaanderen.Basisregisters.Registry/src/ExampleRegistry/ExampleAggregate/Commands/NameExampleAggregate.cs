namespace ExampleRegistry.ExampleAggregate.Commands
{
    public class NameExampleAggregate
    {
        public ExampleAggregateId ExampleAggregateId { get; }

        public ExampleAggregateName ExampleAggregateName { get; }

        public NameExampleAggregate(
            ExampleAggregateId exampleAggregateId,
            ExampleAggregateName exampleAggregateName)
        {
            ExampleAggregateId = exampleAggregateId;
            ExampleAggregateName = exampleAggregateName;
        }
    }
}
