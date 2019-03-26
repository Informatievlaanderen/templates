namespace ExampleRegistry.Projections.Api.ExampleAggregateDetail
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExampleAggregateDetail
    {
        public Guid Id { get; set; }

        public string NameDutch { get; set; }
        public string NameFrench { get; set; }
        public string NameEnglish { get; set; }
        public string NameGerman { get; set; }
    }

    public class ExampleAggregateDetailConfiguration : IEntityTypeConfiguration<ExampleAggregateDetail>
    {
        private const string TableName = "ExampleAggregateDetails";

        public void Configure(EntityTypeBuilder<ExampleAggregateDetail> b)
        {
            b.ToTable(TableName, Schema.Api)
                .HasKey(x => x.Id)
                .ForSqlServerIsClustered(false);

            b.Property(x => x.NameDutch)
                .HasMaxLength(ExampleAggregateName.MaxLength);

            b.Property(x => x.NameFrench)
                .HasMaxLength(ExampleAggregateName.MaxLength);

            b.Property(x => x.NameEnglish)
                .HasMaxLength(ExampleAggregateName.MaxLength);

            b.Property(x => x.NameGerman)
                .HasMaxLength(ExampleAggregateName.MaxLength);

            b.HasIndex(x => x.NameDutch)
                .ForSqlServerIsClustered();
        }
    }
}
