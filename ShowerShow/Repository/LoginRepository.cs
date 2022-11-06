using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private DatabaseContext dbContext;

        public LoginRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> CheckIfCredentialsCorrect(string userName, string password)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());

            if (user.IsNullOrDefault())
            {
                return false;
            }
            else if (!user.PasswordHash.Equals(PasswordHasher.HashPassword(password))){
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
