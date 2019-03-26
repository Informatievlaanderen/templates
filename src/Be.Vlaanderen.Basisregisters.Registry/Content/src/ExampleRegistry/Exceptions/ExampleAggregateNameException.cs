namespace ExampleRegistry.Exceptions
{
    public class ExampleAggregateNameException : ExampleRegistryException
    {
        public ExampleAggregateNameException(string message) : base(message) { }
    }

    public class NoExampleAggregateNameException : ExampleAggregateNameException
    {
        public NoExampleAggregateNameException() : base("ExampleAggregateName cannot be empty.") { }
    }

    public class ExampleAggregateNameTooLongException : ExampleAggregateNameException
    {
        public ExampleAggregateNameTooLongException() : base($"ExampleAggregateName cannot be longer than {ExampleAggregateName.MaxLength} characters.") { }
    }
}
