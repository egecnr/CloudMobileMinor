using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Model
{
    public partial class Disclaimers
    {
        public Guid Id { get; set; }
        public AvailableDisclaimers availableDisclaimers { get; set; }
    }

}
