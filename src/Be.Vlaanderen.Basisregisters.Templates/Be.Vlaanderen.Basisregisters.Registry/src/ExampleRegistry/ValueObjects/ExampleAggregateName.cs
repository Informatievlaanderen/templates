namespace ExampleRegistry
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Exceptions;

    public class ExampleAggregateName : ValueObject<ExampleAggregateName>
    {
        public const int MaxLength = 200;

        public string Name { get; }

        public Language Language { get; }

        public ExampleAggregateName(string name, Language language)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NoExampleAggregateNameException();

            if (name.Length > MaxLength)
                throw new ExampleAggregateNameTooLongException();

            Name = name;
            Language = language;
        }

        protected override IEnumerable<object> Reflect()
        {
            yield return Name;
            yield return Language;
        }

        public override string ToString() => $"{Name} ({Language})";
    }
}
