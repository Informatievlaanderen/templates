namespace ExampleRegistry.Projections.Api
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class ApiProjectionsContext : RunnerDbContext<ApiProjectionsContext>
    {
        public override string ProjectionStateSchema => Schema.Api;

        public DbSet<ExampleAggregateDetail.ExampleAggregateDetail> ExampleAggregateDetails { get; set; }
        public DbSet<ExampleAggregateList.ExampleAggregateList> ExampleAggregateList { get; set; }

        // This needs to be here to please EF
        public ApiProjectionsContext() { }

        // This needs to be DbContextOptions<T> for Autofac!
        public ApiProjectionsContext(DbContextOptions<ApiProjectionsContext> options)
            : base(options) { }
    }
}
