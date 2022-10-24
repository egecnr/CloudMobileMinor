using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Model
{
    public class UserDashboard
    {
        public Guid UserId { get; set; }
        public double TotalWaterUsage { get; set; }
        public double TotalGasUsage { get; set; }
        public double TotalPrice { get; set; }
        public double AvgShowerTime { get; set; }
        public double AvgShowerLiters { get; set; }
        public double AvgShowerGas { get; set; }
        public double AvgShowerPrice { get; set; }
    }
}
