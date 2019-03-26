namespace ExampleRegistry.Api.ExampleAggregate.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation;

    public class DetailExampleAggregateRequest
    {
        /// <summary>Id of the example aggregate to get details for.</summary>
        [Required]
        [Display(Name = "Id")]
        internal Guid? Id { get; set; }
    }

    public class DetailExampleAggregateRequestValidator : AbstractValidator<DetailExampleAggregateRequest>
    {
        public DetailExampleAggregateRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();
        }
    }
}
