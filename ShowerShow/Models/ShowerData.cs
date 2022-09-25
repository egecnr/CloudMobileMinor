using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionApp1.Models
{
    internal class ShowerData
    {
        public Guid Id { get; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; }
        [JsonRequired]
        public double Duration { get; set; }
        [JsonRequired]
        public double WaterUsage { get; set; }
        public double WaterCost { get; set; }
        [JsonRequired]
        public double GasUsage { get; set; }
        public double GasCost { get; set; }
        [JsonRequired]
        public DateTime Date { get; set; }
        public Schedule Schedule { get; set; }
        
    }
}
