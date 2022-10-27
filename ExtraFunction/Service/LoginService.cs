using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;

namespace ExtraFunction.Service
{
    public class LoginService : ILoginService
    {
        private ILoginRepository loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }
        public async Task<bool> CheckIfCredentialsCorrect(string userName, string password)
        {
              return await loginRepository.CheckIfCredentialsCorrect(userName, password);
        }
    }
}
