namespace ExampleRegistry.Exceptions
{
    public class NoExampleAggregateNameException : ExampleRegistryException
    {
        public NoExampleAggregateNameException() : base("ExampleAggregateName cannot be empty.") { }
    }
}
