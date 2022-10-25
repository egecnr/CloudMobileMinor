using ShowerShow.Model;
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

        public Task<Disclaimers> GetDisclaimres(Disclaimers disclaimers)
        {
            return _disclaimerRepository.GetDisclaimres(disclaimers);
        }
        public Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers)
        {
            return _disclaimerRepository.UpdateDisclaimers(disclaimers);
        }

    }
}
