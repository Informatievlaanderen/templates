namespace ExampleRegistry.Api.ExampleAggregate.Responses
{
    using Be.Vlaanderen.Basisregisters.BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class ExampleAggregateNotFoundResponseExamples : IExamplesProvider
    {
        public static string Message = "Non-existing example aggregate.";

        public object GetExamples()
            => new ProblemDetails
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = ProblemDetails.DefaultTitle,
                Detail = Message,
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
