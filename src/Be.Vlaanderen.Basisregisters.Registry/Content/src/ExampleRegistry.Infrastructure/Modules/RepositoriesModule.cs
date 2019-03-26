namespace ExampleRegistry.Infrastructure.Modules
{
    using Autofac;
#if (!ExcludeExample)
    using ExampleAggregate;
    using Repositories;
#endif

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
#if (!ExcludeExample)
            containerBuilder
                .RegisterType<ExampleAggregates>()
                .As<IExampleAggregates>();
#endif
        }
    }
}
