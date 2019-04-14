namespace ExampleRegistry.Projections.Api
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class ApiProjectionsContextMigrationFactory : RunnerDbContextMigrationFactory<ApiProjectionsContext>
    {
        public ApiProjectionsContextMigrationFactory()
            : base("ApiProjectionsAdmin", HistoryConfiguration) { }

        private static MigrationHistoryConfiguration HistoryConfiguration =>
            new MigrationHistoryConfiguration
            {
                Schema = Schema.Api,
                Table = MigrationTables.Api
            };

        protected override ApiProjectionsContext CreateContext(DbContextOptions<ApiProjectionsContext> migrationContextOptions)
            => new ApiProjectionsContext(migrationContextOptions);
    }
}
