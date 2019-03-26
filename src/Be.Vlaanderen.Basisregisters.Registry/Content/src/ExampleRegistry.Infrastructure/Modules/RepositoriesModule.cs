namespace ExampleRegistry.Infrastructure.Modules
{
    using Autofac;
#if (GenerateExample)
    using ExampleAggregate;
    using Repositories;
#endif

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
#if (GenerateExample)
            containerBuilder
                .RegisterType<ExampleAggregates>()
                .As<IExampleAggregates>();
#endif
        }
    }
}
