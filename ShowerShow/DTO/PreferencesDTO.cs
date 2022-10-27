using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShowerShow.Models.Preferences;

namespace ShowerShow.DTO
{
    public class PreferencesDTO
    {
        public string SelectedVoice { get; set; }

        public AvailableLanguages SelectedLanguage { get; set; } = AvailableLanguages.English;

        public AvailableWaterTypes WaterType { get; set; }

        public AvailableCurrencies Currency { get; set; }

        public AvailableThemes Theme { get; set; }

        public bool Notification { get; set; }


    }   
}       
        