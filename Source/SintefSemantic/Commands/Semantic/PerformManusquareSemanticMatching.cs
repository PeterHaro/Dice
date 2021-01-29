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
        
        //TODO: FIX TO WORK ON LINUX CONTAINER. DO INTEROPERABLE
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
            string filepath = Path.Combine(this._hostingEnvironment.WebRootPath, "Matchmaking.jar");
            string args = "/C java -jar " + filepath;
            string standaloneArguments = "java -jar Matchmaking.jar" + " " + HttpUtility.JavaScriptStringEncode(parameter.ToJson());
            string complete_args = args + " " + HttpUtility.JavaScriptStringEncode(parameter.ToJson());

            System.Diagnostics.Process clientProcess = new Process
            {
                StartInfo = {FileName = "java",
                    Arguments = @"-jar " + filepath + " " + HttpUtility.JavaScriptStringEncode(parameter.ToJson()),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            clientProcess.Start();
//            clientProcess.WaitForExit();
            var result = clientProcess.StandardOutput.ReadToEnd();
            var isError = clientProcess.StandardError.ReadToEnd();
            int code = clientProcess.ExitCode;
            Console.WriteLine(code);
            Console.WriteLine();
            
            bool isErr = result.Split('\n').Length > 5;

            return isErr ? new OkObjectResult(isError) : new OkObjectResult(result);
            
            
            //lalalala
            return new OkObjectResult( "ok");
        }
    }
}