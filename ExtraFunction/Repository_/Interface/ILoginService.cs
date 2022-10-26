using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository_.Interface
{
    public interface ILoginService
    {
        Task<bool> CheckIfCredentialsCorrect(string userName, string password);
    }
}
