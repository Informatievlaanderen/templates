namespace ExampleRegistry.Api.ExampleAggregate.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ExampleRegistry.ExampleAggregate.Commands;
    using FluentValidation;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateExampleAggregateRequest
    {
        /// <summary>Id the example aggregate to create.</summary>
        [Required]
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        /// <summary>Name of the example aggregate to create.</summary>
        [Required]
        [Display(Name = "Name")]
        public ExampleAggregateName Name { get; set; }
    }

    public class CreateExampleAggregateRequestValidator : AbstractValidator<CreateExampleAggregateRequest>
    {
        public CreateExampleAggregateRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();

            RuleFor(x => x.Name)
                .Required()
                .SetValidator(new ExampleAggregateNameValidator());
        }
    }

    public class CreateExampleAggregateRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new CreateExampleAggregateRequest
            {
                Id = Guid.NewGuid(),
                Name = new ExampleAggregateName
                {
                    Name = "Something!",
                    Language = Language.English
                }
            };
    }

    public static class CreateExampleAggregateRequestMapping
    {
        public static NameExampleAggregate Map(CreateExampleAggregateRequest message)
            => new NameExampleAggregate(
                new ExampleAggregateId(message.Id.Value),
                new ExampleRegistry.ExampleAggregateName(message.Name.Name, message.Name.Language.Value));
    }
}
