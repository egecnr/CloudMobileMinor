using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ExtraFunction.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using ExtraFunction.Repository_.Interface;
using ExtraFunction.Authorization;

namespace ExtraFunction.Control
{
    public class DisclaimersController
    {
        private readonly ILogger<DisclaimersController> _logger;
        private readonly IDisclaimersRepository _disclaimersRepository;

        public DisclaimersController(ILogger<DisclaimersController> log, IDisclaimersRepository disclaimersRepository)
        {
            _logger = log;
            this._disclaimersRepository = disclaimersRepository;
        }

        [Function(nameof(GetDisclaimers))]
        [OpenApiOperation(operationId: "GetDisclaimers", tags: new[] { "Disclaimers" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Disclaimers), Description = "Successfully received disclaimers")]
        public async Task<HttpResponseData> GetDisclaimers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Disclaimers")] HttpRequestData req) //route is emtpy
        {
            Disclaimers response = await _disclaimersRepository.GetDisclaimers();
            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);

            await responseData.WriteAsJsonAsync(response);
            return responseData;
        }

        //[Function(nameof(UpdateDisclaimers))]
        //[OpenApiOperation(operationId: "UpdateDisclaimers", tags: new[] { "Disclaimers" })]
        //[ExampleAuth]
        //[OpenApiParameter(name: "", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Disclaimers), Description = "Successfully received disclaimeres")]
        //public async Task<HttpResponseData> UpdateDisclaimers([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "")] HttpRequestData req, Disclaimers disclaimers, FunctionContext functionContext)
        //{
        //    var response = _disclaimersRepository.GetDisclaimers(disclaimers);
        //    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);
        //    await responseData.WriteAsJsonAsync(response);
        //    return responseData;
        //}


    }
}
