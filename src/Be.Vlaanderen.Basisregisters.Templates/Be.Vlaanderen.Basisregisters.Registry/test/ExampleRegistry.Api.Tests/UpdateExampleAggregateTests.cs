namespace ExampleRegistry.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using ExampleRegistry.ExampleAggregate.Commands;
    using ExampleAggregate.Requests;
    using FluentValidation.TestHelper;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class UpdateExampleAggregateTests : ApiTest
    {
        public UpdateExampleAggregateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void should_validate_request()
        {
            var validator = new UpdateExampleAggregateRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as ExampleAggregateName);

            var nameValidator = new ExampleAggregateNameValidator();

            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, new string('a', ExampleRegistry.ExampleAggregateName.MaxLength + 10));

            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, null as Language?);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, (Language)666);

            var validRequest = new UpdateExampleAggregateRequest
            {
                Name = new ExampleAggregateName
                {
                    Name = "Bla",
                    Language = Language.English
                }
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Language, validRequest);
        }

        [Fact]
        public async Task should_create_a_correct_command()
        {
            var id = Guid.NewGuid();

            var request = new UpdateExampleAggregateRequest
            {
                Name = new ExampleAggregateName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            var commands = await Put($"/v1/example-aggregates/{id}", request);

            Assert.Single(commands);

            commands[0].IsEqual(
                new NameExampleAggregate(
                    new ExampleAggregateId(id),
                    new ExampleRegistry.ExampleAggregateName(request.Name.Name, request.Name.Language.Value)));
        }

        [Fact]
        public async Task should_fail_on_invalid_id()
        {
            var request = new UpdateExampleAggregateRequest
            {
                Name = new ExampleAggregateName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Put("/v1/example-aggregates/bla", request);

            Assert.Empty(commands);
        }

        [Fact]
        public async Task should_fail_on_invalid_data()
        {
            var request = new UpdateExampleAggregateRequest
            {
                Name = new ExampleAggregateName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Put($"/v1/example-aggregates/{Guid.NewGuid()}", request);

            Assert.Empty(commands);
        }
    }
}
