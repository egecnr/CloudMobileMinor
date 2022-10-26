using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShowerShow.Service;
using ShowerShow.Repository.Interface;
using ShowerShow.Repository;
using Microsoft.Extensions.Configuration;
using ExtraFunction.Repository_.Interface;
using ExtraFunction.Repository_;
using ExtraFunction.Service_;
using ExtraFunction.DAL;
using ExtraFunction.Service;
using ExtraFunction.Repository.Interface;
using ExtraFunction.Authorization;
using ExtraFunction.Repository;

namespace ExtraFunction
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                    .ConfigureFunctionsWorkerDefaults(Worker => Worker.UseNewtonsoftJson().UseMiddleware<JWTMiddleware>())
                    .ConfigureAppConfiguration(config =>
                         config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: false))
                    .ConfigureOpenApi()
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                        services.AddDbContext<DatabaseContext>(options =>
                                   options.UseCosmos("https://sawa-db-fabio.documents.azure.com:443/",
                            "tfGJUagGE3YBw3vCrDhreFiJn0RT0EfnS5NESBJ0ypja5MxfOgRoBFvVUiMoWgurdPzZ1kWcZ1topQrOy5Et7Q==",
                            "sawa-db-fabio"));
                        services.AddTransient<IUserService, UserService>();
                        services.AddTransient<IUserRepository, UserRepository>();
                        services.AddTransient<ILoginService, LoginService>();
                        services.AddTransient<ILoginRepository, LoginRepository>();
                        services.AddSingleton<ITokenService, TokenService>();
                        services.AddTransient<IAchievementRepository, AchievementRepository>();
                        services.AddTransient<IAchievementService, AchievementService>();
                    })
                    .Build();
            await host.RunAsync();

            
        }
    }
}
