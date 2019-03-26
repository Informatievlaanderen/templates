namespace ExampleRegistry
{
    using Autofac;
#if (!ExcludeExampleAggregate)
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using ExampleAggregate;
#endif

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
#if (!ExcludeExampleAggregate)
            containerBuilder
                .RegisterType<ExampleAggregateCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(ExampleAggregateCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
#endif
        }
    }
}
