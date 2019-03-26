namespace ExampleRegistry.Api.ExampleAggregate.Responses
{
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class ExampleAggregateNotFoundResponseExamples : IExamplesProvider
    {
        public static string Message = "Non-existing example aggregate.";

        public object GetExamples() => new BasicApiProblem
        {
            HttpStatus = StatusCodes.Status404NotFound,
            Title = BasicApiProblem.DefaultTitle,
            Detail = Message,
            ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
        };
    }
}