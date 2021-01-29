using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SintefSemantic.Commands;
using SintefSemantic.Commands.Semantic;
using SintefSemantic.Constants;
using SintefSemantic.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SintefSemantic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class InteroperabilityController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = InteroperabilityControllerRoute.OptionsInteroperability)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return this.Ok();
        }
        
        /// <summary>
        /// Performs the Manusquare semantic matching given a specific RFQ
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="rfq">The Manusquare request for quotation.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the semantic matching score or a 404 if it contains an invalid RFQ.</returns>
        [HttpGet("{rfq}", Name = InteroperabilityControllerRoute.GetInteroperability)]
        [HttpHead("{rfq}", Name = InteroperabilityControllerRoute.HeadInteroperability)]
        [SwaggerResponse(StatusCodes.Status200OK, "The semantic matching score for the given RFQ.", typeof(Rfq))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The specific RFQ has been queried earlier has nothing in the system has changed since the date given in the If-Modified-Since HTTP header.", typeof(void))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Invalid format for the RFQ or invalid RFQ per se.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetManusquareAsync(
            [FromServices] IPerformSemanticMatching command,
            string rfq,
            CancellationToken cancellationToken) => command.ExecuteAsync(rfq, cancellationToken);
    }
}