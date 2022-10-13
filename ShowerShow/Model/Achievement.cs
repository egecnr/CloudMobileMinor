using Newtonsoft.Json;
using System;

namespace ShowerShow.Models
{
    //This achievement class along with dtos can be changed to fit the right implementation.

    public class Achievement
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //guid not added in constructor 
        [JsonRequired]
        public string Title { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public int CurrentValue { get; set; } = 0;
        [JsonRequired]
        public int TargetValue { get; set; }


        public Achievement(Guid achId, string title, string description, int currentValue, int targetValue)
        {
            this.Id = achId;
            this.Title = title;
            this.Description = description;
            this.TargetValue = targetValue;
            this.CurrentValue = currentValue;

        }
    }
}
