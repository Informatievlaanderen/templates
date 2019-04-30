namespace ExampleRegistry.Api.Infrastructure
{
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.BasicApiProblem;
    using Microsoft.AspNetCore.Http;

    public class ExampleRegistryExceptionHandler : DefaultExceptionHandler<ExampleRegistryException>
    {
        protected override ProblemDetails GetApiProblemFor(ExampleRegistryException exception) =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status400BadRequest,
                Title = ProblemDetails.DefaultTitle,
                Detail = exception.Message,
                ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
            };
    }
}
