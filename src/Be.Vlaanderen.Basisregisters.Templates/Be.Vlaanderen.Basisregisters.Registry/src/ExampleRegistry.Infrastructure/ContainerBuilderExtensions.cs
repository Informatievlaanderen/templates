namespace ExampleRegistry.Infrastructure
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac;
    using Autofac;
    using Autofac.Core.Registration;
    using Microsoft.Extensions.Configuration;

    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterEventstreamModule(
            this ContainerBuilder builder,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Events");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ApplicationException("Missing 'Events' connectionstring.");

            builder
                .RegisterModule(new SqlStreamStoreModule(connectionString, Schema.Default));

            return builder;
        }

        public static IModuleRegistrar RegisterEventstreamModule(
            this IModuleRegistrar builder,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Events");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ApplicationException("Missing 'Events' connectionstring.");

            return builder
                .RegisterModule(new SqlStreamStoreModule(connectionString, Schema.Default));
        }
    }
}
