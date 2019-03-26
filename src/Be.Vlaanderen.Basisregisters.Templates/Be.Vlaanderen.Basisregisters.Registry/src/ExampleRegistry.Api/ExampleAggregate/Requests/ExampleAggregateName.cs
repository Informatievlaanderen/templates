namespace ExampleRegistry.Api.ExampleAggregate.Requests
{
    using System.ComponentModel.DataAnnotations;
    using FluentValidation;

    public class ExampleAggregateName
    {
        /// <summary>Name of the example aggregate name.</summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>Language of the example aggregate name.</summary>
        [Required]
        [Display(Name = "Language")]
        public Language? Language { get; set; }
    }

    public class ExampleAggregateNameValidator : AbstractValidator<ExampleAggregateName>
    {
        public ExampleAggregateNameValidator()
        {
            RuleFor(x => x.Name)
                .Required()
                .MaxLength(ExampleRegistry.ExampleAggregateName.MaxLength);

            RuleFor(x => x.Language)
                .Required();
        }
    }
}
