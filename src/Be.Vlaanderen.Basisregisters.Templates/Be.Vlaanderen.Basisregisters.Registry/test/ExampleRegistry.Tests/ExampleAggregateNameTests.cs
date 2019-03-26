namespace ExampleRegistry.Tests
{
    using Exceptions;
    using Xunit;

    public class ExampleAggregateNameTests
    {
        [Fact]
        public void cannot_be_empty()
        {
            void NullName() => new ExampleAggregateName(null, Language.Dutch);

            var ex = Record.Exception(NullName);

            Assert.NotNull(ex);
            Assert.IsType<NoExampleAggregateNameException>(ex);
        }

        [Fact]
        public void cannot_be_too_long()
        {
            void LongName() => new ExampleAggregateName(new string('a', ExampleAggregateName.MaxLength + 1), Language.Dutch);

            var ex = Record.Exception(LongName);

            Assert.NotNull(ex);
            Assert.IsType<ExampleAggregateNameTooLongException>(ex);
        }

        [Theory]
        [InlineData("Hallo", Language.Dutch)]
        [InlineData("Hello", Language.English)]
        [InlineData("Bonjour", Language.French)]
        [InlineData("Hai", Language.German)]
        public void must_be_valid(string name, Language language)
        {
            void ValidName() => new ExampleAggregateName(name, language);

            var ex = Record.Exception(ValidName);

            Assert.Null(ex);
        }
    }
}
