namespace ExampleRegistry.Api.ExampleAggregate.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using Infrastructure.Responses;
    using Projections.Api.ExampleAggregateList;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "ExampleAggregates", Namespace = "")]
    public class ExampleAggregateListResponse
    {
        /// <summary>
        /// All example aggregates.
        /// </summary>
        [DataMember(Name = "ExampleAggregates", Order = 1)]
        public List<ExampleAggregateListItemResponse> ExampleAggregates { get; set; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public ExampleAggregateListResponse()
        {
            Links = new List<Link>
            {
                new Link("/", Link.Relations.Home, WebRequestMethods.Http.Get),
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Post)
            };
        }
    }

    [DataContract(Name = "ExampleAggregate", Namespace = "")]
    public class ExampleAggregateListItemResponse
    {
        /// <summary>
        /// Id of the example aggregate.
        /// </summary>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public ExampleAggregateListItemResponse(
            ExampleAggregateList exampleAggregateList)
        {
            Id = exampleAggregateList.Id;

            Links = new List<Link>
            {
                new Link($"/example-aggregates/{exampleAggregateList.Id}", Link.Relations.ExampleAggregate, WebRequestMethods.Http.Get),
            };
        }
    }

    public class ExampleAggregateListResponseExamples : IExamplesProvider
    {
        public object GetExamples()
            => new ExampleAggregateListResponse
            {
                ExampleAggregates = new List<ExampleAggregateListItemResponse>
                {
                    new ExampleAggregateListItemResponse(
                        new ExampleAggregateList
                        {
                            Id = Guid.NewGuid()
                        }),
                    new ExampleAggregateListItemResponse(
                        new ExampleAggregateList
                        {
                            Id = Guid.NewGuid()
                        }),
                }
            };
    }
}
