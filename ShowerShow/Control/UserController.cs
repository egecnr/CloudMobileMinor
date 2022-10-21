using Microsoft.AspNetCore.Mvc;
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
using User = ShowerShow.Models.User;
using CreateUserDTO = ShowerShow.DTO.CreateUserDTO;
using AutoMapper;
using ShowerShow.Repository.Interface;
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShowerShow.Utils;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using HttpMultipartParser;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.RecordIO;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Azure.Storage.Blobs.Models;
using Microsoft.Net.Http.Headers;
using Azure.Core;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace ShowerShow.Controllers
{
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private IUserService userService;

        public UserController(ILogger<UserController> log, IUserService userService)
        {
            _logger = log;
            this.userService = userService;
        }

        [Function("CreateUser")]
        [OpenApiOperation(operationId: "CreateUser", tags: new[] { "Users" })]
        [OpenApiRequestBody("application/json", typeof(CreateUserDTO), Description = "The user data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateUserDTO), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/register")] HttpRequestData req)
        {

            _logger.LogInformation("Creating new user.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CreateUserDTO userDTO = JsonConvert.DeserializeObject<CreateUserDTO>(requestBody);
            if (await userService.CheckIfEmailExist(userDTO.Email))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            else
            {
                await userService.CreateUser(userDTO);
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.Created;

                return responseData;
            }


        }

        [Function("GetUsersByName")]
        [OpenApiOperation(operationId: "GetUsersByName", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUsersByName([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userName}")] HttpRequestData req, string userName)
        {
            _logger.LogInformation($"Fetching users by name {userName}");
            if (!userName.IsNullOrWhiteSpace())
            {
                HttpResponseData responseData = req.CreateResponse();
                IEnumerable<GetUserDTO> users = await userService.GetUsersByName(userName);
                await responseData.WriteAsJsonAsync(users);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
        }

        [Function("GetUser")]
        [OpenApiOperation(operationId: "GetUserById", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetUserDTO), Description = "The OK response with the retrieved user")]
        public async Task<HttpResponseData> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            if (await userService.CheckIfUserExistAndActive(userId))
            {
                GetUserDTO userDTO = await userService.GetUserById(userId);
                HttpResponseData responseData = req.CreateResponse();
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }
        [Function("DeactivateUser")]
        [OpenApiOperation(operationId: "DeactivateUser", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiParameter(name: "isAccountActive", In = ParameterLocation.Path, Required = true, Type = typeof(bool), Description = "Determines if the account is active or not")]
        public async Task<HttpResponseData> DeactivateUser([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/{isAccountActive:bool}")] HttpRequestData req, Guid userId, bool isAccountActive)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            if (await userService.CheckIfUserExist(userId))
            {

                HttpResponseData responseData = req.CreateResponse();
                await userService.DeactivateUserAccount(userId, isAccountActive);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }

        [Function("UpdateUser")]
        [OpenApiOperation(operationId: "UpdateUser", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiRequestBody("application/json", typeof(UpdateUserDTO), Description = "The user data to update.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UpdateUserDTO), Description = "The OK response with the updated user")]
        public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdateUserDTO userDTO = JsonConvert.DeserializeObject<UpdateUserDTO>(requestBody);

            if (await userService.CheckIfUserExistAndActive(userId))
            {
                if (!(await userService.CheckIfEmailExist(userDTO.Email)))
                {
                    HttpResponseData responseData = req.CreateResponse();
                    await userService.UpdateUser(userId, userDTO);
                    responseData.StatusCode = HttpStatusCode.Accepted;
                    return responseData;
                }
                else
                {
                    HttpResponseData responseData = req.CreateResponse();
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }
        [Function("UploadProfilePicture")]
        [OpenApiOperation(operationId: "UploadProfilePicture", tags: new[] { "BlobStorage" })]
        public static async Task<HttpResponseData> UploadProfilePicture([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req, FunctionContext executionContext)
        {
            // get query params
            //var testvalue = executionContext.BindingContext.BindingData["comp"];
            // get form-body        
            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerProfilePictures");
            StreamReader reader = new StreamReader(req.Body);
            var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
            var file = parsedFormBody.Result.Files[0];
            Stream myBlob = reader.BaseStream;
            CloudStorageAccount account = CloudStorageAccount.Parse(Connection);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
            await blockBlob.UploadFromStreamAsync(file.Data);
/*            using (var stream = GenerateStreamFromString(reader.ReadToEnd()))
            {
                byte[] buffer = new byte[file.Data.Length];
                var blobClient = new BlobContainerClient(Connection, containerName);
                var blob = blobClient.GetBlobClient(file.FileName);
                myBlob.Position = 0;

                var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                stream.Position = 0;
                //await blob.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
                
            };*/

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}

