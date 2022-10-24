using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface ITermsAndConditionRepository
    {
        public Task<TermsAndConditions> GetTermsAndConditions(TermsAndConditions termsAndConditions);
        public Task<TermsAndConditions> UpdateTermsAndConditions(TermsAndConditions termsAndConditions);
    }
}
