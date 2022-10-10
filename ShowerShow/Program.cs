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

namespace ShowerShow
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            //for db test
            using (var ct = new DatabaseContext())
            {

                User u1 = new();
                u1.Id = Guid.NewGuid();
                u1.Username = "Todd";
                u1.Friends = new List<UserFriend>();
                UserFriend friend = new UserFriend();
                friend.Id = Guid.NewGuid();
                friend.Username = "Toddsfriend";
                u1.Friends.Add(friend);
                u1.Achievements = new List<Achievement> { new Achievement() };
                u1.UserBadge = "popeye";
                u1.Email = "toddsboringemailaddress";
                u1.Name = "Todd toddler";
                u1.PasswordHash = "Youcantguessthisrightbro";
                ct.Users?.Add(u1);
                await ct.SaveChangesAsync();
            }
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
