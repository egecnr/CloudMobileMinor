﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShowerShow.Models
{
    public class ShowerData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; }

        [JsonRequired]
        public Guid ScheduleId { get; set; }

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
        public int Overtime { get; set; }
        
    }
}
