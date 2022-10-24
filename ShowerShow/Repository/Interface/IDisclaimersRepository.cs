using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public class IDisclaimerRepository
    {
        public Task<Disclaimers> GetDisclaimres(Disclaimers disclaimers);
        public Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers);
    }
}
