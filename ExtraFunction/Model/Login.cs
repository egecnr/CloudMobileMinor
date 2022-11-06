using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Model
{
    [OpenApiExample(typeof(LoginExample))]
    public class Login
    {
        [OpenApiProperty(Description = "Username for the user logging in.")]
        [JsonRequired]
        public string Username { get; set; }

        [OpenApiProperty(Description = "Password for the user logging in.")]
        [JsonRequired]
        public string Password { get; set; }
    }

    public class LoginExample : OpenApiExample<Login>
    {
        public override IOpenApiExample<Login> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("user",
                                                        new Login()
                                                        {
                                                            Username = "username",
                                                            Password = "password"
                                                        },
                                                        NamingStrategy));

            return this;
        }
    }
}

