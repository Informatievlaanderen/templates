namespace ExampleRegistry.Api.ExampleAggregate.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ExampleRegistry.ExampleAggregate.Commands;
    using FluentValidation;
    using Swashbuckle.AspNetCore.Filters;

    public class UpdateExampleAggregateRequest
    {
        /// <summary>Id the example aggregate to update.</summary>
        [Required]
        [Display(Name = "Id")]
        internal Guid? Id { get; set; }

        /// <summary>Name of the example aggregate to update.</summary>
        [Required]
        [Display(Name = "Name")]
        public ExampleAggregateName Name { get; set; }
    }

    public class UpdateExampleAggregateRequestValidator : AbstractValidator<UpdateExampleAggregateRequest>
    {
        public UpdateExampleAggregateRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();

            RuleFor(x => x.Name)
                .Required();
        }
    }

    public class UpdateExampleAggregateRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new UpdateExampleAggregateRequest
            {
                Id = Guid.NewGuid(),
                Name = new ExampleAggregateName
                {
                    Name = "Iets!",
                    Language = Language.Dutch
                }
            };
    }

    public static class UpdateExampleAggregateRequestMapping
    {
        public static NameExampleAggregate Map(UpdateExampleAggregateRequest message)
            => new NameExampleAggregate(
                new ExampleAggregateId(message.Id.Value),
                new ExampleRegistry.ExampleAggregateName(message.Name.Name, message.Name.Language.Value));
    }
}
