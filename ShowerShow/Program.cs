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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShowerShow.Service;
using ShowerShow.Repository.Interface;
using ShowerShow.Repository;
using ShowerShow.Authorization;
using Microsoft.Extensions.Configuration;

namespace ShowerShow
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
                                   options.UseCosmos("https://sawa-db.documents.azure.com:443/",
                            "gggcb28Z24nJAmpz4SRwQRNT9Xyd0wn1riSKAUkvVyaBf4WRALsyx4kgl6POPmi8Ka7JHZfTx06uWD3DHzoqTw==",
                            "sawa-db"));
                        services.AddTransient<IUserService, UserService>();
                        services.AddTransient<IUserRepository, UserRepository>(); 
                        services.AddTransient<IUserFriendService, UserFriendService>();
                        services.AddTransient<IUserFriendRepository, UserFriendRepository>();
                        services.AddTransient<ILoginService, LoginService>();
                        services.AddTransient<ILoginRepository, LoginRepository>();
                        services.AddSingleton<ITokenService, TokenService>();
                    })
                    .Build();


             host.Run();

            
        }
    }
}
