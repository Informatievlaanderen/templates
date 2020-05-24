namespace ExampleRegistry.Api.Infrastructure.Responses
{
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Home", Namespace = "")]
    public class HomeResponse
    {
        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 1)]
        public List<Link> Links { get; set; } = new List<Link>();

        public HomeResponse()
        {
#if (!ExcludeExampleAggregate)
            Links.AddRange(new[]
            {
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Get),
                new Link("/example-aggregates", Link.Relations.ExampleAggregates, WebRequestMethods.Http.Post)
            });
#endif
        }
    }

    public class HomeResponseExamples : IExamplesProvider<HomeResponse>
    {
        public HomeResponse GetExamples() => new HomeResponse();
    }
}
