using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExtraFunction.Repository_.Interface;
using ExtraFunction.Repository_;
using ExtraFunction.Service_;
using ExtraFunction.DAL;
using ExtraFunction.Service;
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
                                   options.UseCosmos("https://sawa-db.documents.azure.com:443/",
                        "gggcb28Z24nJAmpz4SRwQRNT9Xyd0wn1riSKAUkvVyaBf4WRALsyx4kgl6POPmi8Ka7JHZfTx06uWD3DHzoqTw==",
                        "sawa-db"));
                        services.AddTransient<IUserService, UserService>();
                        services.AddTransient<IUserRepository, UserRepository>();
                        services.AddTransient<ILoginService, LoginService>();
                        services.AddTransient<ILoginRepository, LoginRepository>();
                        services.AddSingleton<ITokenService, TokenService>();
                        services.AddTransient<IAchievementRepository, AchievementRepository>();
                        services.AddTransient<IAchievementService, AchievementService>();
                        services.AddTransient<IDisclaimersService, DisclaimersService>();
                        services.AddTransient<IDisclaimersRepository, DisclaimersRepository>();
                        services.AddTransient<ItermsAndConditionsService, TermsAndConditionsService>();
                        services.AddTransient<ITermsAndConditionRepository, TermsAndConditionsRepository>();


                    })
                    .Build();
            await host.RunAsync();

            
        }
    }
}
