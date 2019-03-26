namespace ExampleRegistry
{
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Autofac;
    using ExampleAggregate;

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            // Syntax for commandhandler which do not use SqlStreamStore to store events
            containerBuilder.RegisterType<SimpleExampleCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(SimpleExampleCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();

            // Regular syntax for EventSourcing with SqlStreamStore
            containerBuilder
                .RegisterType<ExampleCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(ExampleCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
        }
    }
}
