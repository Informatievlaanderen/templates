namespace ExampleRegistry.Exceptions
{
    public class NoExampleAggregateIdException : ExampleRegistryException
    {
        public NoExampleAggregateIdException() : base("ExampleAggregateId cannot be empty.") { }
    }
}
