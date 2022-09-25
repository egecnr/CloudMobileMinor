using FunctionApp1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionApp1.DTO
{
    internal class CreateShowerDataDTO
    {

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
        public Schedule Schedule { get; set; } = null;
    }
}
