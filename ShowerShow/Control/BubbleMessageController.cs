using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Models;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using System.Collections.Generic;
using ShowerShow.Model;
using ShowerShow.Service;

namespace ShowerShow.Controllers
{
    public class BubbleMessageController
    {
        private readonly ILogger<BubbleMessageController> _logger;
        private IBubbleMessageService bubbleMessageService;

        public BubbleMessageController(ILogger<BubbleMessageController> log, IBubbleMessageService bubbleMessageService)
        {
            _logger = log;
            this.bubbleMessageService = bubbleMessageService;
        }
        [Function("CreateBubbleMessage")]
        [OpenApiOperation(operationId: "CreateBubbleMessage", tags: new[] { "Bubble Messages" })]
        [OpenApiRequestBody("application/json", typeof(CreateBubbleMessageDTO), Description = "The Bubble Message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(BubbleMessage), Description = "The OK response with the new Bubble Message.")]
        public async Task<HttpResponseData> CreateBubbleMessage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bubble/messages/add")] HttpRequestData req)
        {
            _logger.LogInformation("Creating new bubble message.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            HttpResponseData responseData = req.CreateResponse();
            try
            {

                CreateBubbleMessageDTO data = JsonConvert.DeserializeObject<CreateBubbleMessageDTO>(requestBody);
                await bubbleMessageService.CreateBubbleMessage(data);
                await responseData.WriteAsJsonAsync(data);
                responseData.StatusCode = HttpStatusCode.Created;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;

            }
        }
        [Function("DeleteBubbleMessage")]
        [OpenApiOperation(operationId: "DeleteBubbleMessage", tags: new[] { "Bubble Messages" })]
        [OpenApiParameter(name: "MessageId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The MessageId  parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Schedule), Description = "The OK response with the deleted message")]
        public async Task<HttpResponseData> DeleteSchedule([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "bubble/messages/{MessageId:Guid}/delete")] HttpRequestData req, Guid MessageId)
        {
            _logger.LogInformation("Deleting bubble message.");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await bubbleMessageService.DeleteBubbleMessage(MessageId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetRandomMessage")]
        [OpenApiOperation(operationId: "GetRandomMessage", tags: new[] { "Bubble Messages" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BubbleMessage), Description = "Retrieved message")]
        public async Task<HttpResponseData> GetRandomMessage([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bubble/messages/retrieve")] HttpRequestData req)
        {
            _logger.LogInformation("Getting random message.");
            HttpResponseData responseData = req.CreateResponse();

            try
            {

                BubbleMessage message = await bubbleMessageService.GetRandomBubbleMessage();
                await responseData.WriteAsJsonAsync(message);

                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetBubbleMessageById")]
        [OpenApiOperation(operationId: "GetBubbleMessageById", tags: new[] { "Bubble Messages" })]
        [OpenApiParameter(name: "MessageId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The MessageId parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BubbleMessage), Description = "The OK response with the retrieved bubble message")]
        public async Task<HttpResponseData> GetBubbleMessageById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bubble/messages/retrieve/{MessageId:Guid}")] HttpRequestData req, Guid MessageId)
        {
            _logger.LogInformation("Retrieving message.");


            HttpResponseData responseData = req.CreateResponse();
            try
            {
                BubbleMessage bubbleMessage = await bubbleMessageService.GetBubbleMessageById(MessageId);
                await responseData.WriteAsJsonAsync(bubbleMessage);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch(Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetRandomMessagesList")]
        [OpenApiOperation(operationId: "GetRandomMessagesList", tags: new[] { "Bubble Messages" })]
        [OpenApiParameter(name: "limit", In = ParameterLocation.Path, Required = true, Type = typeof(uint), Description = "The limit parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<BubbleMessage>), Description = "The OK response with the retrieved bubble messages")]
        public async Task<HttpResponseData> GetRandomMessagesList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bubble/messages/get/{limit}")] HttpRequestData req, uint limit)
        {
            _logger.LogInformation("Retrieving message.");


            HttpResponseData responseData = req.CreateResponse();
            try
            {
                List<BubbleMessage> bubbleMessages = (List<BubbleMessage>)await bubbleMessageService.GetListOfRandomBubbleMessages(limit);
                await responseData.WriteAsJsonAsync(bubbleMessages);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
    }
}

