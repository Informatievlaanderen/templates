namespace ExampleRegistry.Exceptions
{
    public class ExampleAggregateIdException : ExampleRegistryException
    {
        public ExampleAggregateIdException(string message) : base(message) { }
    }

    public class NoExampleAggregateIdException : ExampleAggregateIdException
    {
        public NoExampleAggregateIdException() : base("ExampleAggregateId cannot be empty.") { }
    }
}
