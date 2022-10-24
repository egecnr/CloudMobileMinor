using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class TermsAndConditionsService : ItermsAndConditionsService
    {
        private ITermsAndConditionRepository _termsAndConditionRepository;
        public TermsAndConditionsService(ITermsAndConditionRepository termsAndConditionRepository)
        {
            this._termsAndConditionRepository = termsAndConditionRepository;
        }
        public Task<TermsAndConditions> GetTermsAndConditions(TermsAndConditions termsAndConditions)
        {
            return _termsAndConditionRepository.GetTermsAndConditions(termsAndConditions);
        }
        public Task<TermsAndConditions> UpdateTermsAndConditions(TermsAndConditions termsAndConditions)
        {
            return _termsAndConditionRepository.UpdateTermsAndConditions(termsAndConditions);
        }
    }
}
