using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using ShowerShow.Repository;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class DisclaimersService : IDisclaimersService
    {
        private IDisclaimersRepository _disclaimerRepository;

        public DisclaimersService(IDisclaimersRepository disclaimerRepository)
        {
            this._disclaimerRepository = disclaimerRepository;
        }

        public Task<Disclaimers> GetDisclaimers(Disclaimers disclaimers)
        {
            return _disclaimerRepository.GetDisclaimers(disclaimers);
        }
        public Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers)
        {
            return _disclaimerRepository.UpdateDisclaimers(disclaimers);
        }

    }
}
