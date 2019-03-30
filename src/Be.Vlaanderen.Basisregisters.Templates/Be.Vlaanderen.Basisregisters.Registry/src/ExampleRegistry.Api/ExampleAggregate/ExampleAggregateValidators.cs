namespace ExampleRegistry.Api.ExampleAggregate
{
    using System;
    using Be.Vlaanderen.Basisregisters.Api.Localization;
    using Requests;
    using FluentValidation;
    using Microsoft.Extensions.Localization;

    public class ExampleAggregateValidatorsResources
    {
        public string RequiredMessage => "{PropertyName} is required.";
        public string MaxLengthMessage => "{PropertyName} cannot be longer than {MaxLength} characters.";
    }

    public static class ExampleAggregateValidators
    {
        private static readonly IStringLocalizer<ExampleAggregateValidatorsResources> Localizer =
            GlobalStringLocalizer.Instance.GetLocalizer<ExampleAggregateValidatorsResources>();

        public static IRuleBuilderOptions<T, Guid?> Required<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage))
                .NotEqual(Guid.Empty).WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, Language?> Required<T>(this IRuleBuilder<T, Language?> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage))
                .IsInEnum().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, ExampleAggregateName> Required<T>(this IRuleBuilder<T, ExampleAggregateName> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
            => ruleBuilder
                .Length(0, length).WithMessage(Localizer.GetString(x => x.MaxLengthMessage));
    }
}
