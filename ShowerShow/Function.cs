using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Logging;
using ShowerShow;

namespace ShowerShow
{
    public static class Function
    {
        [FunctionName("Function")]
        public static void Run([CosmosDBTrigger(
            databaseName: "testdb",
            collectionName: "Users",
            ConnectionStringSetting = "DBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
