using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using ShowerShow.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowerShow.Models;
using User = ShowerShow.Models.User;
using Microsoft.Extensions.DependencyInjection;
using ShowerShow.Repository.Interfaces;
using ShowerShow.Repository;
using ShowerShow.Service;

namespace ShowerShow
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                    .ConfigureFunctionsWorkerDefaults()
                    .ConfigureOpenApi()
                    .ConfigureServices(services =>
                    {
                        services.AddTransient<IAchievementRepository, AchievementRepository>();
                        services.AddTransient<IAchievementService, AchievementService>();
                        services.AddDbContext<DatabaseContext>();

                    })
                    .Build();
            await host.RunAsync();

            
        }
    }
}
