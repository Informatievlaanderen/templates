namespace ExampleRegistry.Exceptions
{
    using System;

    public class NoExampleIdException : ExampleRegistryException
    {
        public NoExampleIdException() { }

        public NoExampleIdException(string message) : base(message) { }

        public NoExampleIdException(string message, Exception inner) : base(message, inner) { }
    }
}
