namespace ExampleRegistry.Api.Tests.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Api.Infrastructure;
    using Api.Infrastructure.Configuration;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Swagger;

    public class TestStartup
    {
        private IContainer _applicationContainer;

        private readonly IConfiguration _configuration;

        public TestStartup(
            IConfiguration configuration) =>
            _configuration = configuration;

        /// <summary>Configures services for the application.</summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureDefaultForApi<Startup>(
                    (provider, description) => new Info
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = "Example Registry API",
                        Description = GetApiLeadingText(description),
                        Contact = new Contact
                        {
                            Name = "agentschap Informatie Vlaanderen",
                            Email = "informatie.vlaanderen@vlaanderen.be",
                            Url = "https://vlaanderen.be/informatie-vlaanderen"
                        }
                    },
                    new[] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name, },
                    corsHeaders: _configuration.GetSection("Cors").GetChildren().Select(c => c.Value).ToArray(),
                    configureFluentValidation: fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            app.UseDefaultForApi(new StartupOptions
            {
                ApplicationContainer = _applicationContainer,
                ServiceProvider = serviceProvider,
                HostingEnvironment = env,
                ApplicationLifetime = appLifetime,
                LoggerFactory = loggerFactory,
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = groupName => $"Example Registry API {groupName}",
                    CustomExceptionHandlers = new IExceptionHandler[]
                    {
                        new ValidationExceptionHandling()
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
                },
            });
        }

        private static string GetApiLeadingText(ApiVersionDescription description)
            => $"Right now you are reading the documentation for version {description.ApiVersion} of the Example Registry API{string.Format(description.IsDeprecated ? ", **this API version is not supported any more**." : ".")}";
    }
}
