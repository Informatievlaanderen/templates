namespace ExampleRegistry.Tests
{
    using System;
    using Exceptions;
    using Xunit;

    public class ExampleAggregateIdTests
    {
        [Fact]
        public void cannot_be_empty()
        {
            void EmptyId() => new ExampleAggregateId(Guid.Empty);

            var ex = Record.Exception(EmptyId);

            Assert.NotNull(ex);
            Assert.IsType<NoExampleAggregateIdException>(ex);
        }

        [Theory]
        [InlineData("69e7cc43-abc0-4c75-8fa5-dd119afb6bef")]
        [InlineData("5de7ce6c-d168-42d0-9552-6b9cb24cf3ae")]
        [InlineData("c139b971-b577-4d6e-97b8-fd0f01a75c81")]
        [InlineData("23686d0e-cc47-4824-955b-7131fa780ad6")]
        [InlineData("7ea0fe4a-e714-436b-8ea1-3d32043ae8c9")]
        public void must_be_valid(string id)
        {
            void ValidId() => new ExampleAggregateId(new Guid(id));

            var ex = Record.Exception(ValidId);

            Assert.Null(ex);
        }
    }
}
