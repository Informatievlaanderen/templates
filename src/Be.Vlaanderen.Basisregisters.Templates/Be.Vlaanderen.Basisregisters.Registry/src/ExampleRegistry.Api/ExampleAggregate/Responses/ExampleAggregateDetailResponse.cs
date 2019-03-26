namespace ExampleRegistry.Api.ExampleAggregate.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using Infrastructure.Responses;
    using Projections.Api.ExampleAggregateDetail;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "ExampleAggregate", Namespace = "")]
    public class ExampleAggregateDetailResponse
    {
        /// <summary>Id of the example aggregate.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public ExampleAggregateDetailResponse(
            ExampleAggregateDetail exampleAggregateDetail)
        {
            Id = exampleAggregateDetail.Id;

            Links = new List<Link>
            {
                new Link("/", Link.Relations.Home, WebRequestMethods.Http.Get),
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Get)
            };
        }
    }

    public class ExampleAggregateDetailResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new ExampleAggregateDetailResponse(
                new ExampleAggregateDetail
                {
                    Id = Guid.NewGuid()
                });
    }
}
