using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExtraFunction.Service
{
    public class DisclaimersService : IDisclaimersService
    {
        private IDisclaimersRepository _disclaimerRepository;

        public DisclaimersService(IDisclaimersRepository disclaimerRepository)
        {
            this._disclaimerRepository = disclaimerRepository;
        }
        public Task<Disclaimers> GetDisclaimers()
        {
            return _disclaimerRepository.GetDisclaimers();
        }
        public Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers)
        {
            return _disclaimerRepository.UpdateDisclaimers(disclaimers);
        }

        

    }
}
