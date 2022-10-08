using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ShowerShow.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShowerShow
{
    public class Startup : IWebJobsStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IWebJobsBuilder builder)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>

            options.UseCosmos("https://database-sawa.documents.azure.com:443/"
            , "0iV6DDVOqBso4R7ylBYskYk7vPhYtzoQS8kg7ltSdAuTY7xpXLlHtCZAh3au9qDoEOPw4lE91jVApTkQrHLB8g=="
            , databaseName: "Database - SAWA"));

        }
    }
}