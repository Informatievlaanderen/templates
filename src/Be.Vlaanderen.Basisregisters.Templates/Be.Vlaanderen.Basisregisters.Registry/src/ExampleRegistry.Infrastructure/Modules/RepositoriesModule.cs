namespace ExampleRegistry.Infrastructure.Modules
{
    using Autofac;
#if (!ExcludeExampleAggregate)
    using ExampleAggregate;
    using Repositories;
#endif

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
#if (!ExcludeExampleAggregate)
            containerBuilder
                .RegisterType<ExampleAggregates>()
                .As<IExampleAggregates>();
#endif
        }
    }
}
