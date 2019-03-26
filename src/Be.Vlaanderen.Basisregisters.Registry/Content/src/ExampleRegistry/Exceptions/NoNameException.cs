namespace ExampleRegistry.Exceptions
{
    using System;

    public class NoNameException : ExampleRegistryException
    {
        public NoNameException() { }

        public NoNameException(string message) : base(message) { }

        public NoNameException(string message, Exception inner) : base(message, inner) { }
    }
}
