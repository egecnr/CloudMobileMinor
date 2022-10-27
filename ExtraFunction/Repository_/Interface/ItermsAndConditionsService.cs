using ExtraFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository_.Interface
{
    internal interface ItermsAndConditionsService
    {
        public Task<TermsAndConditions> GetTermsAndConditions();
        public Task<TermsAndConditions> UpdateTermsAndConditions(TermsAndConditions termsAndConditions);
    }
}
