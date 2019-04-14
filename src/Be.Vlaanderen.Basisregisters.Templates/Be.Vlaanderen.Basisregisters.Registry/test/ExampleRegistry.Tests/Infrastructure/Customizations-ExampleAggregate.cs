namespace ExampleRegistry.Tests.Infrastructure
{
    using AutoFixture;

    public static partial class Customizations
    {
        public static void CustomizeExampleAggregateName(this IFixture fixture) =>
            fixture.Customize<ExampleAggregateName>(composer =>
                composer.FromFactory(generator =>
                    new ExampleAggregateName(new string(
                            (char) generator.Next(97, 123), // a-z
                            generator.Next(1, ExampleAggregateName.MaxLength)),
                        Language.Dutch)));
    }
}
