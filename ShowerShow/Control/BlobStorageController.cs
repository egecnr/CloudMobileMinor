using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Repository.Interface;
using System.Collections.Generic;
using HttpMultipartParser;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Linq;
using ShowerShow.Authorization;

namespace ShowerShow.Controllers
{
    public class BlobStorageController
    {
        private readonly ILogger<BlobStorageController> _logger;
        // get env connection string
        private string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private CloudStorageAccount account;

        private IUserService userService;

        public BlobStorageController(ILogger<BlobStorageController> log, IUserService userService)
        {
            _logger = log;
            account = CloudStorageAccount.Parse(connection);
            this.userService = userService;
        }


        [Function("UploadProfilePicture")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiOperation(operationId: "UploadProfilePicture", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{UserId:Guid}/profile/uploadpic")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Uploading to blob.");

            string containerName = Environment.GetEnvironmentVariable("ContainerProfilePictures");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (!await userService.CheckIfUserExistAndActive(UserId))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
                var file = parsedFormBody.Result.Files[0];


                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(UserId.ToString() + ".png");
                blockBlob.Properties.ContentType = file.ContentType;

                if (!blockBlob.Properties.ContentType.Contains("image"))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

                await blockBlob.UploadFromStreamAsync(file.Data);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
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

            string containerName = Environment.GetEnvironmentVariable("ContainerProfilePictures");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                if (!await userService.CheckIfUserExistAndActive(UserId))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);


                CloudBlockBlob blockBlob = container.GetBlockBlobReference(UserId.ToString() + ".png");
                if (!await container.ExistsAsync())
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                if (!await blockBlob.ExistsAsync())
                {
                    blockBlob = container.GetBlockBlobReference("defaultpicture.png");
                    if (!await blockBlob?.ExistsAsync())
                    {
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        return responseData;
                    }
                }
                responseData = GetDownloadResponseData(responseData, blockBlob, "image/jpeg").Result;
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }

        }
        [Function("UploadVoiceSound")] // USE POSTMAN TO TEST
        [ExampleAuth]
        [OpenApiOperation(operationId: "UploadVoiceSound", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadVoiceSound([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/defaultvoices")] HttpRequestData req, FunctionContext functionContext)
        {
            _logger.LogInformation("Uploading to blob.");

            string containerName = Environment.GetEnvironmentVariable("ContainerDefaultVoices");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
                var file = parsedFormBody.Result.Files[0];


                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
                blockBlob.Properties.ContentType = file.ContentType;

                if (!blockBlob.Properties.ContentType.Contains("audio"))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

                await blockBlob.UploadFromStreamAsync(file.Data);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
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

            string containerName = Environment.GetEnvironmentVariable("ContainerDefaultVoices");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);


                CloudBlockBlob blockBlob = container.GetBlockBlobReference(FileName);
                if (!await container.ExistsAsync() || !await blockBlob.ExistsAsync())
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                responseData = GetDownloadResponseData(responseData, blockBlob, "audio/mpeg").Result;
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
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
            string containerName = Environment.GetEnvironmentVariable("ContainerProfilePictures");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                if (!await userService.CheckIfUserExistAndActive(UserId))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);


                CloudBlockBlob blockBlob = container.GetBlockBlobReference(UserId.ToString() + ".png");
                if (!await container.ExistsAsync() || !await blockBlob.ExistsAsync())
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                await blockBlob.DeleteIfExistsAsync();
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
        }

        [Function("GetAllVoices")] // THIS IS NOT WORKING
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
        }
        public async Task<HttpResponseData> GetDownloadResponseData(HttpResponseData responseData, CloudBlockBlob blockBlob, string ContentType)
        {
            byte[] content = null;
            using (MemoryStream ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);
                content = ms.ToArray();
                responseData.WriteBytes(content);
                responseData.Headers.Add("Content-Type", ContentType);
                responseData.Headers.Add("Accept-Ranges", $"bytes");
                responseData.Headers.Add("Content-Disposition", $"attachment; filename={blockBlob.Name}; filename*=UTF-8'{blockBlob.Name}");
            }
            return responseData;
        }
    }

}