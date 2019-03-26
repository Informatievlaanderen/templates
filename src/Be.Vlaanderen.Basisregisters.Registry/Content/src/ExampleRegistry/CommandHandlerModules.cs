namespace ExampleRegistry
{
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Autofac;
    using ExampleAggregate;

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<ExampleAggregateCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(ExampleAggregateCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
        }
    }
}
