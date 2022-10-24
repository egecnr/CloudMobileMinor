using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShowerShow.Authorization;
using ShowerShow.Controllers;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Control
{
    public class DisclaimersController
    {
        private readonly ILogger<ShowerController> _logger;
        private readonly IDisclaimersRepository _disclaimersRepository;

        public DisclaimersController(ILogger<ShowerController> log, IDisclaimersRepository disclaimersRepository)
        {
            _logger = log;
            this._disclaimersRepository = disclaimersRepository;
        }

        [Function(nameof(GetDisclaimers))]
        [OpenApiOperation(operationId: "GetDisclaimers", tags: new[] { "Terms and Conditions" })]
        [ExampleAuth]
        [OpenApiParameter(name: "", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerData), Description = "Successfully received disclaimeres")]
        public async Task<HttpResponseData> GetDisclaimers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "")] HttpRequestData req, Disclaimers disclaimers, FunctionContext functionContext)
        {
            var response = _disclaimersRepository.GetDisclaimres(disclaimers);
            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(response);
            return responseData;
        }

        [Function(nameof(UpdateDisclaimers))]
        [OpenApiOperation(operationId: "UpdateDisclaimers", tags: new[] { "Terms and Conditions" })]
        [ExampleAuth]
        [OpenApiParameter(name: "", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerData), Description = "Successfully received disclaimeres")]
        public async Task<HttpResponseData> UpdateDisclaimers([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "")] HttpRequestData req, Disclaimers disclaimers, FunctionContext functionContext)
        {
            var response = _disclaimersRepository.GetDisclaimres(disclaimers);
            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(response);
            return responseData;
        }


    }
}
