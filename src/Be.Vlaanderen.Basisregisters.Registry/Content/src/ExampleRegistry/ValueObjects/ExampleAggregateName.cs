namespace ExampleRegistry
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Exceptions;

    public class ExampleAggregateName : ValueObject<ExampleAggregateName>
    {
        public string Name { get; }

        public Language Language { get; }

        public ExampleAggregateName(string name, Language language)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NoExampleAggregateNameException();

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
