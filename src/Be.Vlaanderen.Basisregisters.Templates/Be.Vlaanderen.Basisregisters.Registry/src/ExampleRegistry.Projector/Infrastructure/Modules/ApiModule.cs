namespace ExampleRegistry.Projector.Infrastructure.Modules
{
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore.Autofac;
    using Be.Vlaanderen.Basisregisters.Projector;
    using Be.Vlaanderen.Basisregisters.Projector.Modules;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ExampleRegistry.Infrastructure;
    using ExampleRegistry.Projections.Api;
#if (!ExcludeExampleAggregate)
    using ExampleRegistry.Projections.Api.ExampleAggregateDetail;
    using ExampleRegistry.Projections.Api.ExampleAggregateList;
#endif

    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly ILoggerFactory _loggerFactory;

        public ApiModule(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _services = services;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterModule(new LoggingModule(_configuration, _services))
                .RegisterModule(new DataDogModule(_configuration));

            RegisterProjectionSetup(builder);

            builder.Populate(_services);
        }

        private void RegisterProjectionSetup(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                    new EventHandlingModule(
                        typeof(DomainAssemblyMarker).Assembly,
                        EventsJsonSerializerSettingsProvider.CreateSerializerSettings()))

                .RegisterModule<EnvelopeModule>()

                .RegisterEventstreamModule(_configuration)

                .RegisterModule<ProjectorModule>();

            RegisterApiProjections(builder);
        }

        private void RegisterApiProjections(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                    new ApiProjectionsModule(
                        _configuration,
                        _services,
                        _loggerFactory));

#if (!ExcludeExampleAggregate)
            builder
                .RegisterProjectionMigrator<ApiProjectionsContextMigrationFactory>(
                    _configuration,
                    _loggerFactory)

                .RegisterProjections<ExampleAggregateDetailProjections, ApiProjectionsContext>()
                .RegisterProjections<ExampleAggregateListProjections, ApiProjectionsContext>();
#else
            builder
                .RegisterProjectionMigrator<ApiProjectionsContextMigrationFactory>(
                    _configuration,
                    _loggerFactory);
#endif
        }
    }
}
