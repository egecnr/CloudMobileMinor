﻿using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
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
