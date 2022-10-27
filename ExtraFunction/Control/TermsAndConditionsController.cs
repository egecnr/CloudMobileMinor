using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ExtraFunction.Repository_.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExtraFunction.Authorization;
using ExtraFunction.Model;

namespace ExtraFunction.Control
{
    public class TermsAndConditionsController
    {
        private readonly ILogger<TermsAndConditionsController> _logger;
        private readonly ITermsAndConditionRepository _termsAndConditionRepository;

        public TermsAndConditionsController(ILogger<TermsAndConditionsController> log, ITermsAndConditionRepository termsAndConditionRepository)
        {
            _logger = log;
            this._termsAndConditionRepository = termsAndConditionRepository;
        }

        [Function(nameof(GetTermsAndConditions))]
        [OpenApiOperation(operationId: "GetTermsAndCondition", tags: new[] { "Terms and Conditions" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TermsAndConditions), Description = "Successfully received terms and conditions")]
        public async Task<HttpResponseData> GetTermsAndConditions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TermsAndCondition")] HttpRequestData req)
        {
            TermsAndConditions response = await _termsAndConditionRepository.GetTermsAndConditions();
            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(response);
            return responseData;
        }


        //[Function(nameof(UpdateTermsAndCondition))]
        //[OpenApiOperation(operationId: "UpdateTermsAndCondition", tags: new[] {"Terms and Conditions"})]
        //[ExampleAuth]
        //[OpenApiParameter(name: "", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TermsAndConditions), Description = "Successfully updated terms and conditions")]
        //public async Task<HttpResponseData> UpdateTermsAndCondition([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "")] HttpRequestData req, TermsAndConditions termsAndConditions,FunctionContext functionContext)
        //{

        //    var response = _termsAndConditionRepository.UpdateTermsAndConditions(termsAndConditions);
        //    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);
        //    await responseData.WriteAsJsonAsync(response);
        //    return responseData;

        //}


    }
}
