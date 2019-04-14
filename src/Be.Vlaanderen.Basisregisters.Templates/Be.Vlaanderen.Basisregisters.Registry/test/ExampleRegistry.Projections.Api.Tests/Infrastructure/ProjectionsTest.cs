namespace ExampleRegistry.Projections.Api.Tests.Infrastructure
{
    using Xunit.Abstractions;

    public class ProjectionsTest
    {
        protected ITestOutputHelper TestOutputHelper { get; private set; }

        public ProjectionsTest(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;

    }
}
