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

    public class CreateExampleAggregateTests : ApiTest
    {
        public CreateExampleAggregateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void should_validate_request()
        {
            var validator = new CreateExampleAggregateRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.Id, null as Guid?);
            validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as ExampleAggregateName);

            var nameValidator = new ExampleAggregateNameValidator();

            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, new string('a', ExampleRegistry.ExampleAggregateName.MaxLength + 10));

            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, null as Language?);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, (Language)666);

            var validRequest = new CreateExampleAggregateRequest
            {
                Id = Guid.NewGuid(),
                Name = new ExampleAggregateName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Language, validRequest);
        }

        [Fact]
        public async Task should_create_a_correct_command()
        {
            var request = new CreateExampleAggregateRequest
            {
                Id = Guid.NewGuid(),
                Name = new ExampleAggregateName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            var commands = await Post("/v1/example-aggregates", request);

            Assert.Single(commands);

            commands[0].IsEqual(
                new NameExampleAggregate(
                    new ExampleAggregateId(request.Id.Value),
                    new ExampleRegistry.ExampleAggregateName(request.Name.Name, request.Name.Language.Value)));
        }

        [Fact]
        public async Task should_fail_on_invalid_data()
        {
            var request = new CreateExampleAggregateRequest
            {
                Id = Guid.NewGuid(),
                Name = new ExampleAggregateName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Post("/v1/example-aggregates", request);

            Assert.Empty(commands);
        }
    }
}
