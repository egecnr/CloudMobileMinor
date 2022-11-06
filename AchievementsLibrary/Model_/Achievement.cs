using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Model_
{
    public class Achievement
    {
        [JsonRequired]
        public string Title { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public int CurrentValue { get; set; } = 0;
        [JsonRequired]
        public int TargetValue { get; set; }


        public Achievement(string title, string description, int currentValue, int targetValue)
        {

            this.Title = title;
            this.Description = description;
            this.TargetValue = targetValue;
            this.CurrentValue = currentValue;
        }


        public static List<Achievement> InitializedAchievements()
        {
            List<Achievement> achievements = new List<Achievement>();

            achievements.Add(new Achievement("Perfect week", "Using sawa as 7 times a week", 0, 7)); //string, string, int,int
            achievements.Add(new Achievement("Great week", "Using sawa 5 times a week", 0, 5));
            achievements.Add(new Achievement("Perfect month", "Using Sawa 28 times/month", 0, 28));
            achievements.Add(new Achievement("Great month", "Using Sawa 20 times/month", 0, 20));
            achievements.Add(new Achievement("Perfect quarter", "Using sawa 84 times/quarter", 0, 84));
            achievements.Add(new Achievement("Great quarter", "Using Sawa 60 times/quarter", 0, 60));
            achievements.Add(new Achievement("Perfect Year", "Using Sawa 336 times/year", 0, 336));
            achievements.Add(new Achievement("Great Year", "Using Sawa 240 times/year", 0, 240));
            achievements.Add(new Achievement("Fresh frog", "Showering (partly) cold at least five times in a week", 0, 5));
            achievements.Add(new Achievement("Early Bird", "Shower before 8:00 ", 0, 1));
            achievements.Add(new Achievement("Early Bird Bonanza", "Shower before 8:00 five times a week", 0, 5));
            achievements.Add(new Achievement("Night Owl", "Shower past 22:00", 0, 1));
            achievements.Add(new Achievement("Night Albatros", "Shower past 22:00 five times a week", 0, 5));
            achievements.Add(new Achievement("Flash Shower", "Shower in less than five minutes", 0, 5));

            return achievements;

        }

    }
}
