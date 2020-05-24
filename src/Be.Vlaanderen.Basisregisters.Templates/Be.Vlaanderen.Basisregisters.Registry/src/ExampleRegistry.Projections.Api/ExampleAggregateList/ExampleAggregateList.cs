namespace ExampleRegistry.Projections.Api.ExampleAggregateList
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExampleAggregateList
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
    }

    public class ExampleAggregateListConfiguration : IEntityTypeConfiguration<ExampleAggregateList>
    {
        private const string TableName = "ExampleAggregateList";

        public void Configure(EntityTypeBuilder<ExampleAggregateList> b)
        {
            b.ToTable(TableName, Schema.Api)
                .HasKey(x => x.Id)
                .IsClustered(false);

            b.Property(x => x.Name)
                .HasMaxLength(ExampleAggregateName.MaxLength)
                .IsRequired(false);

            b.HasIndex(x => x.Name)
                .IsClustered();
        }
    }
}
