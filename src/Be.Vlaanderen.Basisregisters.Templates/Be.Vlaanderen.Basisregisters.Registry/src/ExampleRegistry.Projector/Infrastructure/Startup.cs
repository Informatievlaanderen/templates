namespace ExampleRegistry.Projector.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.AspNetCore;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.Projector.ConnectedProjections;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Modules;
    using SqlStreamStore;
    using Swashbuckle.AspNetCore.Swagger;
    using TraceSource = Be.Vlaanderen.Basisregisters.DataDog.Tracing.TraceSource;

    /// <summary>Represents the startup process for the application.</summary>
    public class Startup
    {
        private IContainer _applicationContainer;

        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        /// <summary>Configures services for the application.</summary>
        /// <param aggregateName="services">The collection of services to configure the application with.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            const string defaultCulture = "en-GB";
            const string supportedCultures = "en-GB;en-US;en;nl-BE;nl;fr-BE;fr";

            services
                .ConfigureDefaultForApi<Startup, SharedResources>(new StartupConfigureOptions
                {
                    Cors =
                    {
                        Headers = _configuration
                            .GetSection("Cors")
                            .GetChildren()
                            .Select(c => c.Value)
                            .ToArray()
                    },
                    Swagger =
                    {
                        ApiInfo = (provider, description) => new Info
                        {
                            Version = description.ApiVersion.ToString(),
                            Title = "Example Registry Projector API",
                            Description = GetApiLeadingText(description),
                            Contact = new Contact
                            {
                                Name = "agentschap Informatie Vlaanderen",
                                Email = "informatie.vlaanderen@vlaanderen.be",
                                Url = "https://vlaanderen.be/informatie-vlaanderen"
                            }
                        },
                        XmlCommentPaths = new [] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name }
                    },
                    Localization =
                    {
                        DefaultCulture = new CultureInfo(defaultCulture),
                        SupportedCultures = supportedCultures
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => new CultureInfo(x))
                            .ToArray()
                    },
                    MiddlewareHooks =
                    {
                        FluentValidation = fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()
                    }
                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApiModule(_configuration, services, _loggerFactory));
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider,
            ApiDataDogToggle datadogToggle,
            ApiDebugDataDogToggle debugDataDogToggle,
            MsSqlStreamStore streamStore)
        {
            StartupHelpers.EnsureSqlStreamStoreSchema<Startup>(streamStore, loggerFactory);

            if (datadogToggle.FeatureEnabled)
            {
                if (debugDataDogToggle.FeatureEnabled)
                    StartupHelpers.SetupSourceListener(serviceProvider.GetRequiredService<TraceSource>());

                app.UseDataDogTracing(
                    serviceProvider.GetRequiredService<TraceSource>(),
                    _configuration["DataDog:ServiceName"],
                    pathToCheck => pathToCheck != "/");
            }

            app.UseDefaultForApi(new StartupUseOptions
            {
                Common =
                {
                    ApplicationContainer = _applicationContainer,
                    ServiceProvider = serviceProvider,
                    HostingEnvironment = env,
                    ApplicationLifetime = appLifetime,
                    LoggerFactory = loggerFactory,
                },
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = groupName => $"Example Registry Projector API {groupName}",
                    CustomExceptionHandlers = new IExceptionHandler[]
                    {
                        new ValidationExceptionHandling(),
                    }
                },
                Server =
                {
                    PoweredByName = "Vlaamse overheid - Basisregisters Vlaanderen",
                    ServerName = "agentschap Informatie Vlaanderen"
                },
                MiddlewareHooks =
                {
                    AfterMiddleware = x => x.UseMiddleware<AddNoCacheHeadersMiddleware>(),
                }
            });

            var projectionsManager = serviceProvider.GetRequiredService<IConnectedProjectionsManager>();
            projectionsManager.Start();
        }

        private static string GetApiLeadingText(ApiVersionDescription description)
            => $"Right now you are reading the documentation for version {description.ApiVersion} of the Example Registry Projector API{string.Format(description.IsDeprecated ? ", **this API version is not supported any more**." : ".")}";
    }
}
