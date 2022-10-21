using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<bool> CheckIfCredentialsCorrect(string userName,string password);
    }
}
