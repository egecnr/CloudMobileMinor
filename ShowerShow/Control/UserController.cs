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

namespace ShowerShow.Controllers
{
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private IUserService userService;

        public UserController(ILogger<UserController> log,IUserService userService)
        {
            _logger = log;
            this.userService = userService;
        }

        [Function("CreateUser")]
        [OpenApiOperation(operationId: "CreateUser", tags: new[] { "Users" })]
        [OpenApiRequestBody("application/json", typeof(CreateUserDTO), Description = "The user data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/register")] HttpRequestData req)
        {

            _logger.LogInformation("Creating new user.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                CreateUserDTO userDTO = JsonConvert.DeserializeObject<CreateUserDTO>(requestBody);
            if(await userService.CheckIfEmailExist(userDTO.Email))
            {
                 HttpResponseData responseData = req.CreateResponse();
                 responseData.StatusCode = HttpStatusCode.BadRequest;
                  return responseData;
            }
            else
            {
                await userService.CreateUser(userDTO);
                HttpResponseData responseData = req.CreateResponse();
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.StatusCode = HttpStatusCode.Created;
                
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
                if (await userService.CheckIfUserExist(userId))
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
    }
}

