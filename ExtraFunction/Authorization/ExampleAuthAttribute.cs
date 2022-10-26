using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Authorization
{
    public class ExampleAuthAttribute : OpenApiSecurityAttribute
    {
        public ExampleAuthAttribute() : base("ExampleAuth", SecuritySchemeType.Http)
        {
            Description = "JWT for authorization";
            In = OpenApiSecurityLocationType.Header;
            Scheme = OpenApiSecuritySchemeType.Bearer;
            BearerFormat = "JWT";
        }
    }
}
