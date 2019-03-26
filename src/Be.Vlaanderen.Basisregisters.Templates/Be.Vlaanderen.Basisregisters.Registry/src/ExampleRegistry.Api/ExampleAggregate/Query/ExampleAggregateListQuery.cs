namespace ExampleRegistry.Api.ExampleAggregate.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Microsoft.EntityFrameworkCore;
    using Projections.Api;
    using Projections.Api.ExampleAggregateList;

    public class ExampleAggregateListQuery : Query<ExampleAggregateList, ExampleAggregateListFilter>
    {
        private readonly ApiProjectionsContext _context;

        protected override ISorting Sorting => new DomainSorting();

        public ExampleAggregateListQuery(ApiProjectionsContext context) => _context = context;

        protected override IQueryable<ExampleAggregateList> Filter(FilteringHeader<ExampleAggregateListFilter> filtering)
        {
            var exampleAggregates = _context
                .ExampleAggregateList
                .AsNoTracking();

            if (!filtering.ShouldFilter)
                return exampleAggregates;

            if (filtering.Filter.Id.HasValue)
                exampleAggregates = exampleAggregates.Where(m => m.Id == filtering.Filter.Id.Value);

            return exampleAggregates;
        }

        internal class DomainSorting : ISorting
        {
            public IEnumerable<string> SortableFields { get; } = new[]
            {
                nameof(ExampleAggregateList.Id),
            };

            public SortingHeader DefaultSortingHeader { get; } = new SortingHeader(nameof(ExampleAggregateList.Id), SortOrder.Ascending);
        }
    }

    public class ExampleAggregateListFilter
    {
        public Guid? Id { get; set; }
    }
}
