using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                })
                .Build();
            await host.RunAsync();
        }
    }
}
