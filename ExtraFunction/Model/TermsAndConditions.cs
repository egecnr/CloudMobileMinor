using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtraFunction.Model;

namespace ExtraFunction.Model
{
    public class TermsAndConditions
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserRestrictions { get; set; }
        public string RulesOfConduct { get; set; }
        public string ContactInformation { get; set; }
        public DateTime Date { get; set; }
    }
}
