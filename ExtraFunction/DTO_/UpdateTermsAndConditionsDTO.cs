using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.DTO_
{
    public class UpdateTermsAndConditionsDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserRestrictions { get; set; }
        public string RulesOfConduct { get; set; }
        public string ContactInformation { get; set; }
        public DateTime Date { get; set; }
    }
}
