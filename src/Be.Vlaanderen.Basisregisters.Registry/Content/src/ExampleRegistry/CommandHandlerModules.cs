namespace ExampleRegistry
{
    using Autofac;
#if (!ExcludeExample)
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using ExampleAggregate;
#endif

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
#if (!ExcludeExample)
            containerBuilder
                .RegisterType<ExampleAggregateCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(ExampleAggregateCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
#endif
        }
    }
}
