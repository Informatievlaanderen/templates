namespace ExampleRegistry
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public abstract class ExampleRegistryException : DomainException
    {
        protected ExampleRegistryException() { }

        protected ExampleRegistryException(string message) : base(message) { }

        protected ExampleRegistryException(string message, Exception inner) : base(message, inner) { }
    }
}
