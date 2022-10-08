using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ShowerShow.DTO;
using ShowerShow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShowerShow.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.WebJobs.Hosting;
using ShowerShow;

namespace ShowerShow.Controllers
{
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private DatabaseContext databaseContext = new DatabaseContext(new DbContextOptions<DatabaseContext>());

        public UserController(ILogger<UserController> log)
        {
            _logger = log;
        }

        [FunctionName("CreateUser")]
        [OpenApiOperation(operationId: "CreateUser", tags: new[] { "Users" })]
        [OpenApiRequestBody("application/json", typeof(CreateUserDTO), Description = "The user data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/register")] HttpRequest req)
        {

            _logger.LogInformation("Creating new user.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                CreateUserDTO data = JsonConvert.DeserializeObject<CreateUserDTO>(requestBody);

                User user = new();
                user.Name = data.Name;
                user.Username = data.Username;
                user.Email = data.Email;
                user.PasswordHash = data.PasswordHash;
                databaseContext.Users.Add(user);


                return new OkObjectResult(user);
            }
            catch (Exception ex)
            {
                // DEV ONLY
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

