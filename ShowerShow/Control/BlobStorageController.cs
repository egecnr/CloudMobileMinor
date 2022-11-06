using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Repository.Interface;
using ShowerShow.Authorization;

namespace ShowerShow.Controllers
{
    public class BlobStorageController
    {
        private readonly ILogger<BlobStorageController> _logger;
        private IBlobStorageService blobStorageService;

        public BlobStorageController(ILogger<BlobStorageController> log, IBlobStorageService blobStorageService)
        {
            _logger = log;
            this.blobStorageService = blobStorageService;
        }


        [Function("UploadProfilePicture")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiOperation(operationId: "UploadProfilePicture", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{UserId:Guid}/profile/uploadpic")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await blobStorageService.UploadProfilePicture(req.Body, UserId);
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
        [Function("GetProfilePictureOfUser")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiOperation(operationId: "GetProfilePictureOfUser", tags: new[] { "Blob Storage" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpeg", bodyType: typeof(byte[]), Description = "The OK response with the profile picture.")]
        public async Task<HttpResponseData> GetProfilePictureOfUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/profile/getpic")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                responseData = await blobStorageService.GetProfilePictureOfUser(responseData, UserId);
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
        [Function("UploadVoiceSound")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiOperation(operationId: "UploadVoiceSound", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadVoiceSound([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/defaultvoices")] HttpRequestData req, FunctionContext functionContext)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                await blobStorageService.UploadVoiceSound(req.Body);
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
        [Function("GetVoiceSound")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiParameter(name: "FileName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The file name parameter")]
        [OpenApiOperation(operationId: "GetVoiceSound", tags: new[] { "Blob Storage" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "audio/mpeg", bodyType: typeof(byte[]), Description = "The OK response with the voice.")]
        public async Task<HttpResponseData> GetVoiceSound([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/defaultvoices/{FileName}")] HttpRequestData req, string FileName, FunctionContext functionContext)
        {
            _logger.LogInformation("Downloading from blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                responseData = await blobStorageService.GetVoiceSound(responseData, req.Body, FileName);
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
        [Function("DeleteUserProfilePicture")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "DeleteUserProfilePicture", tags: new[] { "Blob Storage" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        public async Task<HttpResponseData> DeleteUserProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{UserId:Guid}/profile/deletepic")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Deleting user profile picture.");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                await blobStorageService.DeleteProfilePicture(UserId);
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
      /*  [Function("GetAllVoices")] // THIS IS NOT WORKING
        [OpenApiOperation(operationId: "GetAllVoices", tags: new[] { "Blob Storage" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "audio/mpeg", bodyType: typeof(byte[]), Description = "The OK response with the voice.")]
        public async Task<HttpResponseData> GetAllVoices([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/defaultvoices/getall")] HttpRequestData req)
        {
            _logger.LogInformation("Downloading from blob.");

            string containerName = Environment.GetEnvironmentVariable("ContainerDefaultVoices");

            try
            {
                HttpResponseData responseData = req.CreateResponse();
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                if (!await container.ExistsAsync())
                {

                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(currentToken: null);
                List<IListBlobItem> blobItems = resultSegment.Results.ToList();


                using (MemoryStream ms = new MemoryStream())
                {
                    foreach (IListBlobItem blobItem in blobItems)
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)blobItem;
                        await blob.FetchAttributesAsync();
                        byte[] content = null;
                        HttpResponseData rd = req.CreateResponse();
                        await blob.DownloadToStreamAsync(ms);
                        content = ms.ToArray();
                        await rd.WriteBytesAsync(content);
                        rd.Headers.Add("Content-Type", "audio/mpeg");
                        rd.Headers.Add("Accept-Ranges", $"bytes");
                        rd.Headers.Add("Content-Disposition", $"attachment; filename={blob.Name}; filename*=UTF-8'{blob.Name}");
                    }
                }
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
        }*/
    }

}