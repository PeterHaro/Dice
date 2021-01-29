using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SintefSemantic.Models;

namespace SintefSemantic.Commands.Semantic
{
    public class PerformManusquareSemanticMatching : l
    {
        private readonly IActionContextAccessor actionContextAccessor;

        public PerformManusquareSemanticMatching(IActionContextAccessor actionContextAccessor)
        {
            this.actionContextAccessor = actionContextAccessor;
        }
        
        public async Task<IActionResult> ExecuteAsync(string requestForQuotation, CancellationToken cancellationToken)
        {
            Rfq rfq = Rfq.FromJson(requestForQuotation);
            var httpContext = this.actionContextAccessor.ActionContext.HttpContext;
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out StringValues stringValues))
            {
                //TODO: ADD WHEN SYSTEM WAS LAST UPDATED
                
       /*         if (DateTimeOffset.TryParse(stringValues, out var modifiedSince) &&
                    (modifiedSince >= car.Modified))
                {
                    return new StatusCodeResult(StatusCodes.Status304NotModified);
                } */
            }
            //Perform matching here:
            
            
            
            //lalalala
            return new OkObjectResult( "ok");
        }
    }
}