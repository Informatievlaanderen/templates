namespace ExampleRegistry.Api.ExampleAggregate
{
    using System;
    using Requests;
    using FluentValidation;

    public static class ExampleAggregateValidators
    {
        public static IRuleBuilderOptions<T, Guid?> Required<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
            => ruleBuilder
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("{PropertyName} is required.");

        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

        public static IRuleBuilderOptions<T, Language?> Required<T>(this IRuleBuilder<T, Language?> ruleBuilder)
            => ruleBuilder
                .NotEmpty()
                .IsInEnum()
                .WithMessage("{PropertyName} is required.");

        public static IRuleBuilderOptions<T, ExampleAggregateName> Required<T>(this IRuleBuilder<T, ExampleAggregateName> ruleBuilder)
            => ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
            => ruleBuilder
                .Length(0, length)
                .WithMessage("{PropertyName} cannot be longer than {MaxLength}.");
    }
}
