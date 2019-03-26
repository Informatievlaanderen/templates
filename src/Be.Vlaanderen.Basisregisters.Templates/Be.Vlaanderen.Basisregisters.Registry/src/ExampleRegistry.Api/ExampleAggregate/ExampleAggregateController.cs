namespace ExampleRegistry.Api.ExampleAggregate
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Pagination;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Converters;
    using Projections.Api;
    using Projections.Api.ExampleAggregateDetail;
    using Query;
    using Requests;
    using Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("example-aggregates")]
    [ApiExplorerSettings(GroupName = "ExampleAggregate")]
    public class ExampleAggregateController : ExampleRegistryController
    {
        /// <summary>
        /// List example aggregates.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If example aggregates are found.</response>
        /// <response code="500">If an internal error has occurred.</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ExampleAggregateListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleAggregateListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> ListExampleAggregates(
            [FromServices] ApiProjectionsContext context,
            CancellationToken cancellationToken = default)
        {
            var filtering = Request.ExtractFilteringRequest<ExampleAggregateListFilter>();
            var sorting = Request.ExtractSortingRequest();
            var pagination = Request.ExtractPaginationRequest();

            var pagedExampleAggregates = new ExampleAggregateListQuery(context)
                .Fetch(filtering, sorting, pagination);

            Response.AddPagedQueryResultHeaders(pagedExampleAggregates);

            return Ok(
                new ExampleAggregateListResponse
                {
                    ExampleAggregates = await pagedExampleAggregates
                        .Items
                        .Select(x => new ExampleAggregateListItemResponse(x))
                        .ToListAsync(cancellationToken)
                });
        }

        /// <summary>
        /// Get details of the example aggregate.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exampleAggregateId">Identificator of the example aggregate.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If the example aggregate is found.</response>
        /// <response code="400">If the request contains invalid data.</response>
        /// <response code="404">If the example aggregate does not exist.</response>
        /// <response code="500">If an internal error has occured.</response>
        [HttpGet("{exampleAggregateId}")]
        [ProducesResponseType(typeof(ExampleAggregateDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BasicApiValidationProblem), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(typeof(BasicApiProblem), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleAggregateDetailResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleAggregateNotFoundResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> DetailExampleAggregate(
            [FromServices] ApiProjectionsContext context,
            [FromRoute] Guid exampleAggregateId,
            CancellationToken cancellationToken = default)
        {
            var request = new DetailExampleAggregateRequest
            {
                Id = exampleAggregateId,
            };

            await new DetailExampleAggregateRequestValidator()
                .ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var exampleAggregate = await FindExampleAggregateAsync(context, request.Id.Value, cancellationToken);

            return Ok(
                new ExampleAggregateDetailResponse(exampleAggregate));
        }

        private static async Task<ExampleAggregateDetail> FindExampleAggregateAsync(
            ApiProjectionsContext context,
            Guid exampleAggregateId,
            CancellationToken cancellationToken)
        {
            var exampleAggregate = await context
                .ExampleAggregateDetails
                .FindAsync(new object[] { exampleAggregateId }, cancellationToken);

            if (exampleAggregate == null)
                throw new ApiException(ExampleAggregateNotFoundResponseExamples.Message, StatusCodes.Status404NotFound);

            return exampleAggregate;
        }
    }
}
